using CodeFactory.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFactory.AzureTableRepository.Logic
{
	public static class ProjectExtensions
	{

    /// <summary>
    /// Used to check a project model for the existence of a folder at the root level of a given name.  If the folder is 
    /// missing - create it.
    /// </summary>
    /// <param name="source">The visual studio project that we are checking exists or creating.</param>
    /// <param name="folderName">The name of the folder to return.</param>
    /// <returns>The existing or created project folder.</returns>
    /// <exception cref="ArgumentNullException">Thrown if either provided parameter is not provided.</exception>
    public static async Task<VsProjectFolder> CheckAddFolder(this VsProject source, string folderName)
    {
      //Bounds checking to make sure all the data needed to get the folder returned is provided.
      if (source == null) throw new ArgumentNullException(nameof(source));
      if (string.IsNullOrEmpty(folderName)) throw new ArgumentNullException(nameof(folderName));

      //Calling the project system in CodeFactory and getting all the children in the root of the project.
      var projectFolders = await source.GetChildrenAsync(false);

      //Searching for the project folder, if it is not found will add the project folder to the root of the project.
      return projectFolders.Where(m => m.ModelType == VisualStudioModelType.ProjectFolder)
                 .Where(m => m.Name.Equals(folderName))
                 .Cast<VsProjectFolder>()
                 .FirstOrDefault()
             ?? await source.AddProjectFolderAsync(folderName);

    }
  }
}
