# Contributing to CodeFactory Command Automation Packages 

### First off, thanks for taking the time to contribute! 

The following is a set of guidelines for contributing to CodeFactory and its open source community-driven automation packages, which are referenced in the [Public Projects](https://github.com/CodeFactoryLLC/Public-Projects) repo on GitHub. These are mostly guidelines, not rules. Use your best judgment, and feel free to propose changes to this document in a pull request. 

### Code of Conduct 

This project and everyone participating in it is governed by the CodeFactory [Code of Conduct](https://github.com/CodeFactoryLLC/Public-Projects/blob/master/Code_Of_Conduct.md). By participating, you are expected to uphold this code. Please report unacceptable behavior to support@codefactory.software. 

***
#### I don't want to read this whole thing I just have a question!!! 

Note: Please don't file an issue to ask a question. You'll get faster results by submitting your questions on our [Support Page](https://www.codefactory.software/support).
***

# What should I know before I get started? 

### CodeFactory and Community-authored Automation Packages 

While the CodeFactory RT client is a commercial product, a wide variety of open-source Automation Commands are publicly available for you to use, clone, modify, etc.  

In order to author a new automation template, or modify an existing template, you will need to have both an active CodeFactory Runtime client installed (as an extension into your copy of Visual Studio 2019). Additionally, you will need to download and install the CodeFactory SDK into the same install of Visual Studio that you have your CodeFacory Runtime installed into.

* [Request Trial Copy of CodeFactory RT Here](https://www.codefactory.software/try-codefactory)

* [Download the CodeFactory SDK on NuGet](https://www.nuget.org/packages/CodeFactorySDK/) 

Once those two dependencies are taken care of, you will have the ability to create a File -> New -> CodeFactory - Commands Library project. This template along with its sister, CodeFactory - Automation Library are included with Visual Studio from the CodeFactory SDK installer.

### What is a CodeFactory "Automation Command"?

The way in which a CodeFactory Automation template works with your code is via one or more Commands that are defined within a Command Library. Currently - there are seven (7) different kinds of commands that CodeFactory makes available to authors. 

#### Command Types
Name | Description
-----|-------
Solution | Like its name suggests, this command is used when an author wishes to begin code automation logic while starting at the top-most node (ie. Solution) of the Visual Studio Solution Explorer. Please see here for more details about this command.
Solution Document | This command is used to begin a command from any document that lives at the Solution level within Visual Studio. Please see Solution Document for more details.
Solution Folder | This command sets the context for a code automation at a folder node within the Visual Studio Explorer hierarchy. Please see Solution Folder for more details.
Project | This command sets the context for a code automation at the Project node within the Visual Studio Solution Explorer window. Please see Project Command for details.
Project Folder | This command sets the context for a code automation at Folder within a Project node in the Visual Studio Solution Explorer. Please see Project Folder for more details.
Project Document | This command is for any node within a Project hierarchy that is not a folder or the project node itself. Project documents can be anything that you are allowed to add into a project per normal Visual Studio rules. Examples include; xml, config, png, bmp, js, html, java, ps, etc. Please see Project Document for more details.
CSharp Source | This command is a special case of the Project Document case. CodeFactory will pass into your command class a real-time model of any C# source code artifacts found within a *.cs file in the target Solution. Usually these files are found under a Project Node within the Solution Explorer - but not always. Please see C# Source for more details.

[Learn more here](http://docs.codefactory.software/guidance/overview-commands-intro.html).

# How Can I Contribute? 

### Accessing #HelpWanted Issues
We've created a few issues tagged with #HelpWanted to indicate where we think there would be significant interest once an automation has been authorided and published. We monitor the [Issue List](https://github.com/CodeFactoryLLC/Public-Projects/issues) for any questions or support needs you might have to get started building your own custom CodeFactory Automation Command packages.

### Build New or Improve Existing Packages?

A good primer on how to build a publish your own CodeFactory Automation Command is available [here](http://docs.codefactory.software/guidance/usage-intro.html).

When [Designing Software Factories with CodeFactory](http://docs.codefactory.software/guidance/design/intro.html)
the possibilities of code automation are only limited by the .NET framework and the imagination of the automation template author. 

We've identified two basic categories that have seperate design constraints and intents. These are Greenfield and Refactor scenarios.

#### Greenfield Code Automation
These types of code automation templates will generate brand new code artifacts from a model or an interface. We have found during testing that the most effective use of these kinds of templates focus on enforcing code discipline and patterns that have been selected by Architecture or Designers of the target system. Examples of these kind of templates would be:

* Generate a Service (JSON, WebService, WCF, etc) from an Entity or POCO object
* Build/insert logging code into a class(es)
* Build/insert/enforce Exception handling into your codebase
* Build all of the service registration code intelligently in a target project
* Build an object mapper extension method between an internal model and an external source (DB, service, etc)

#### Refactor Code Automation
Refactor code automation templates allow the Code Automation template author to build a series of Automation Commands that are designed for converting one kind of code artifact into another. You can look at several of the reference projects that our team has shared for examples like #WebFormsToBlazor. Other examples might include:

* Migration WebForms *.aspx code to Blazor
* Migration older versions of .NET framework code to a newer version
* Update ASP.MVC
* Pull the logic from a web application and turn it into a set of Micro-Services
* Dynamically identify Data Layer service code and re-write it for a different storage platform (Oracle->Microsoft, Relational -> NoSQL, etc)

### Reporting Bugs 

This section guides you through submitting a bug report for open source content. Following these guidelines helps maintainers and the community understand your report, reproduce the behavior, and find related reports. 

Before creating bug reports, please check [this list] as you might find out that you don't need to create one. When you are creating a bug report, please include as many details as possible. Fill out the required template, the information it asks for helps us resolve issues faster. 

Note: If you find a Closed issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one. 

### Before Submitting A Bug Report 

Check the FAQs on the forum for a list of common questions and problems. 

Determine which repository the problem should be reported in. 

Perform a cursory search to see if the problem has already been reported. If it has and the issue is still open, add a comment to the existing issue instead of opening a new one. 
