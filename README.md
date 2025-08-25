# GitExtensions.PluginManager
NuGet based plugin manager for Git Extensions

![Preview](/assets/screenshot-search.png)

Repository with some example plugins - https://www.myget.org/F/neptuo-gitextensions/api/v2.<br>
Nightly builds of PluginManager available at Appveyor - https://ci.appveyor.com/nuget/gitextensions-pluginmanager.

### Appveyor

[![Build status](https://ci.appveyor.com/api/projects/status/cg2y7qojbugw7usg?svg=true)](https://ci.appveyor.com/project/gitextensions/gitextensions-pluginmanager)

### Browsing for packages
PluginManager integrated into Git Extensions filters packages with dependency on `GitExtensions.Extensibility`. This package is right now a kind of meta package and it is used to mark nuget packages intended as Git Extensions plugins.

### Command line arguments
PluginManager is designed to be a reusable tool to manage plugins distributed as nuget packages. This repository contains an integration package for Git Extensins.
As a standalone application, PluginManager supports these command line arguments:

- `--path` (required) - A root path to a directory where to install packages.
- `--selfpackageid` (optional) - A package id to indicate which package should be treated as a package for self update. 
- `--dependencies` (optional) - A comma separated list of package ids and versions that are required in package to be compatible (Eg. `GitExtensions.Extensibility-v3.0,TestA,TestB-v1`).
- `--monikers` (optional) - A comma separated list of .NET framework monikers to filter package content during extraction (Eg. `net461,netstandard2.0`).
- `--processnamestokillbeforechange` - A comma separated list of process names to be killed before any changes being processed (it is used to kill all instances on Git Extensions before installing/uninstalling dlls, that might be locked).

### Binding between GitExtensions.PluginManager and GitExtensions.Extensibility

The plugin defines a required dependency on `GitExtensions.Extensibility` (see [Plugin.cs#L55](https://github.com/gitextensions/gitextensions.pluginmanager/blob/master/src/GitExtensions.PluginManager/Plugin.cs#L55)) which filters out packages that are not meant as plugins for Git Extensions. The idea is that by selecting minimal version, we limit plugins targeting a specific Git Extensions version. Each version of Git Extensions that is not compatible with the previous one in terms of plugins, should increment a Major version (before we stabilize GitExtensions.Extensibility, we are incrementing Minor version).

### Icons

Some icons by Yusuke [Kamiyamane](http://p.yusukekamiyamane.com).<br>
Some other by [Material Design](https://material.io/tools/icons).
