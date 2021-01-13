﻿# CodeFactory.AzureTableRepository

This CodeFactory action will allow you to connect to an existing Azure Storage Table and generate two files to help make working with that table easier:

- A C# entity class that inherits from TableEntity
- A partial repository class that inherits from our BaseTableRepository class and allows you to perform standard create, read, update, and delete operations against the Azure Storage Table

There is a very simple dialog included that will allow you to specify an Azure Storage connection to interact with.

We welcome contributions to make this more robust and provide more interactions with the tables on Azure
