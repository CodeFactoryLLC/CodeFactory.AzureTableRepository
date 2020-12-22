using CodeFactory.AzureTableRepository.Logic;
using CodeFactory.Logging;
using CodeFactory.VisualStudio;
using CodeFactory.VisualStudio.UI;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeFactory.AzureTableRepository.ExplorerCommands.Project.Dialog
{
	/// <summary>
	/// Interaction logic for RepositoryConfigurationDialog.xaml
	/// </summary>
	public partial class RepositoryConfigurationDialog : VsUserControl
	{
		/// <summary>
		/// Creates an instance of the user control.
		/// </summary>
		/// <param name="vsActions">The visual studio actions that are accessible by this user control.</param>
		/// <param name="logger">The logger used by this user control.</param>
		public RepositoryConfigurationDialog(IVsActions vsActions, ILogger logger) : base(vsActions, logger)
		{
			//Initializes the controls on the screen and subscribes to all control events (Required for the screen to run properly)
			InitializeComponent();
		}

		public string AzureStorageConnectionString { get; set; }

		public VsProject Project { get; internal set; }

		public void GetTables() {

			var account = CloudStorageAccount.Parse(AzureStorageConnectionString);
			var client = account.CreateCloudTableClient();
			var tables = client.ListTables().ToArray();
			var firstEntry = tables[0].CreateQuery<DynamicTableEntity>().First();
			AddToTables(tables[0].Name, firstEntry);

		}

		private static string[] TableProperties = new string[] { "PartitionKey", "RowKey", "ETag", "Timestamp" };
		private void AddToTables(string name, DynamicTableEntity firstEntry)
		{

			var properties = firstEntry.Properties.Where(p => !TableProperties.Contains(p.Key));
			var tbl = new AzureTable
			{
				Name = name,
				Fields = properties.Select(p => (p.Key, p.Value.PropertyType)).ToArray()
			};
			Tables.Add(tbl);

		}

		public List<AzureTable> Tables { get; set; } = new List<AzureTable>();

		public class AzureTable {

			public string Name { get; set; }

			public (string Name, EdmType type)[] Fields { get; set; }

		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{

			GetTables();

			GenerateClassesForTable(Tables[0]);
			GenerateRepositoryForTable(Tables[0]);

		}

		private void GenerateRepositoryForTable(AzureTable tbl)
		{
			
			var folder = Project.CheckAddFolder("Models").GetAwaiter().GetResult();
			if (!Project.GetChildrenAsync(true).GetAwaiter().GetResult().Any(n => n.Name == "BaseRepository.cs")) {
				// Add baserepository
				folder.AddDocumentAsync("BaseRepository.cs", BaseRepositorySource.Replace("<<NAMESPACE>>", $"{Project.DefaultNamespace}.Models"));
			}

			var fmt = new CodeFactory.SourceFormatter();
			fmt.AppendCodeLine(0, $"namespace {Project.DefaultNamespace}.Models {{");
			fmt.AppendCodeLine(0);
			fmt.AppendCodeLine(1, $"public partial class {tbl.Name}Repository : BaseTableRepository<{tbl.Name}> {{ }}");
			fmt.AppendCodeLine(0);
			fmt.AppendCodeLine(0, "}");

			folder.AddDocumentAsync($"{tbl.Name}Repository.cs", fmt.ReturnSource());

		}

		private void GenerateClassesForTable(AzureTable tbl)
		{

			var fmt = new CodeFactory.SourceFormatter();

			fmt.AppendCodeLine(0, "using Microsoft.Azure.Cosmos.Table;");
			fmt.AppendCodeLine(0);
			fmt.AppendCodeLine(0, $"namespace {Project.DefaultNamespace}.Models {{");
			fmt.AppendCodeLine(0);
			fmt.AppendCodeLine(1, $"public partial class {tbl.Name} : TableEntity {{");
			fmt.AppendCodeLine(0);

			foreach (var f in tbl.Fields)
			{
				fmt.AppendCodeLine(2, $"public {TypeMappings[f.type]} {f.Name} {{ get; set; }}");
				fmt.AppendCodeLine(0);
			}

			fmt.AppendCodeLine(1, "}");
			fmt.AppendCodeLine(0);
			fmt.AppendCodeLine(0, "}");

			var folder = Project.CheckAddFolder("Models").GetAwaiter().GetResult();
			folder.AddDocumentAsync($"{tbl.Name}.cs", fmt.ReturnSource());

		}

		private static readonly Dictionary<EdmType, string> TypeMappings = new Dictionary<EdmType, string>
		{
			{EdmType.Binary, "byte[]" },
			{EdmType.Boolean, "bool" },
			{EdmType.DateTime, "DateTime" },
			{EdmType.Double, "double" },
			{EdmType.Guid, "Guid" },
			{EdmType.Int32, "int" },
			{EdmType.Int64, "long" },
			{EdmType.String, "string" }
		};


		private void Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private static readonly string BaseRepositorySource = @"using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace <<NAMESPACE>>
{

	public abstract class BaseTableRepository<T> where T : TableEntity, new()
	{

		protected IConfiguration _Configuration { get; }

		protected virtual string TableName { get { return typeof(T).Name; } }

		public virtual string ConnectionString { get; set; }

		protected CloudTable GetCloudTable(string tableName)
		{

			var account = CloudStorageAccount.Parse(ConnectionString);
			var client = account.CreateCloudTableClient(new TableClientConfiguration());
			client.GetTableReference(tableName).CreateIfNotExists();
			return client.GetTableReference(tableName);


		}

	public virtual async Task AddOrUpdate(T obj)
	{

		if (obj is ISetKeys keyObj) keyObj.SetKeys();

		var table = GetCloudTable(TableName);

		try
		{
			var result = await table.ExecuteAsync(TableOperation.InsertOrReplace(obj));
		}
		catch (StorageException ex)
		{
			throw new Exception(""Error from Azure Storage: "" + ex.RequestInformation.ExtendedErrorInformation.ErrorMessage, ex);
		}

	}

	public async Task<T> Get(string partitionKey, string rowKey)
	{

		var table = GetCloudTable(TableName);
		var getOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

		return (await table.ExecuteAsync(getOperation)).Result as T;

	}

	public T GetByRowKey(string rowKey)
	{

		var table = GetCloudTable(TableName);
		var query = new TableQuery<T>
		{
			FilterString = TableQuery.GenerateFilterCondition(""RowKey"", QueryComparisons.Equal, rowKey)
		};

		return table.ExecuteQuery(query).FirstOrDefault();

	}

	public async Task<IEnumerable<T>> GetAllForPartition(string partitionKey)
	{

		var table = GetCloudTable(TableName);

		var query = new TableQuery<T>
		{
			FilterString = TableQuery.GenerateFilterCondition(""PartitionKey"", QueryComparisons.Equal, partitionKey)
		};

		TableContinuationToken token = null;
		var outList = new List<T>();
		while (true)
		{
			var results = await table.ExecuteQuerySegmentedAsync<T>(query.Take(10), token);
			if (results.Results.Count == 0) break;

			outList.AddRange(results.Results);

			if (results.ContinuationToken != null)
			{
				token = results.ContinuationToken;
			}
			else
			{
				break;
			}

		}

		return outList;

	}


	public Task Remove(T sub)
	{

		if (sub == null) return Task.CompletedTask;

		var table = GetCloudTable(TableName);

		return table.ExecuteAsync(TableOperation.Delete(sub));

	}


}

public interface ISetKeys
{
	void SetKeys();
}

}
";

	}
}
