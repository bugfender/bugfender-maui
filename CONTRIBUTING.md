# Contributing
If you would like to contribute to this SDK's development, please follow these steps.

## Requisites
 * .NET MAUI (to install, [follow the instructions](https://learn.microsoft.com/en-us/dotnet/maui/get-started/first-app))

## Pull requests
If you'd like to contribute some code, please feel free to open a pull request.

Here are some tips for your PR to be successful:

* Contact Bugfender beforehand with your proposal, to align on the changes needed and how you're planning to implement them.
* Avoid breaking the end user's API if not necessary.
* Align the style of your code with the code of the rest of the repository.
* Make the minimal changes necessary, for example, avoid changing parts of the code you're not touching or avoid changing code only because of style.
* Provide some proof of testing: for example, with screenshots of your code working or with unit/integration tests.
* Avoid using external libraries whenever possible.
* Read the project's license.

If you open a pull request, you're granting the project maintainers the right to use your code, under the current or any other license.

## Common tasks

### Updating the native libraries

This repository contains the Bugfender iOS and Android SDKs, which can be updated anytime and maybe are not updated here. At the moment of writing this, the SDKs used are:

* Android 3.2.0
* iOS 2.0.0

### Updating iOS

Follow these steps for updating:

* Download the latest version of the [iOS SDK from GitHub](https://github.com/bugfender/BugfenderSDK-iOS); you need the `.framework` file. Add it to the `Binding.iOS` project.
* Update `Binding.iOS/ApiDefinition.cs` by using [Objective Sharpie](https://developer.xamarin.com/guides/cross-platform/macios/binding/objective-sharpie/). Manually check which are the methods updated and merge them. For reference, the invocation looks something like this: `sharpie bind BugfenderSDK.framework/Headers/BugfenderSDK.h -scope BugfenderSDK.framework/Headers/ -sdk iphoneos12.1`
* Update the `Binding.Sdk.iOS/BugfenderBinding.cs` file if the API changed.
* Update this file to reflect the version.

### Updating Android

Follow these steps:

* Download the latest version of the [Android SDK from Maven Central](http://search.maven.org/#search%7Cga%7C1%7Cbugfender); you need the `aar` file. Add it to the `Binding.Android` project.
* Update the `Binding.Sdk.Android/BugfenderBinding.cs` file if the API changed.
* Update this file to reflect the version.

### Testing with local sample

Build the NuGet with:

```shell
dotnet build --configuration Release
nuget pack
```

Add it to a local repo and import it from the Sample application for testing:

```shell
nuget add *.nupkg -source ~/nugetrepo
```

Remove from the local repo and clean the cache:

```shell
nuget delete Bugfender.Sdk 3.x.x -source ~/nugetrepo
nuget locals all -clear
```