# What is MSMQ.Messaging

MSMQ.Messaging is intended to be a straight port of the .NET Framework's System.Messaging assembly to .NET Core, based on the [reference source](https://github.com/microsoft/referencesource).  Since MSMQ is platform specific, it will only run on Windows, and should work with .NET Core 3.1 and up.  I originally created this project so that I could move legacy code to .NET Core, and it has only been tested with relatively simple use-cases.  

The code that this is derived from is [awaiting merge](https://github.com/microsoft/referencesource/pull/141) into the main branch of [reference source](https://github.com/microsoft/referencesource). Until that is complete, the licensing status is ambiguous, and you use this at your own risk.  For now, you will have to build from source if you want to use the code, I anticipate sumbitting to nuget as soon as possible.

## Getting Started

It should be pretty straightforward: clone the source and dotnet build it. There's also a .sln file for use with Visual Studio 2019.  

1. MSMQ.Messaging relies on mqrt.dll to do the work, so while it will run on .NET Core, it will only do so on Windows.
2. All but the installation *should* work at this point, but test coverage is limited right now.
3. Configuring queues may or may not work, I don't know -- use cases I have don't require them.
4. API references -- [System.Messaging docs](https://docs.microsoft.com/en-us/dotnet/api/system.messaging?view=netframework-4.8) for classic .NET Framework should work for you, I have no intention of adding/changing any behavior.  I'm not an MSMQ person, I just want to be able to move some code that relies on MSMQ from the .NET Framework to .NET Core.  

## Upgrading .NET Framework projects

MSMQ.Messaging is intended to be simple to use if you are replacing System.Messaging code.  You should only have to change the references and rename System.Messaging to MSMQ.Messaging in your usings section.  Everything else should remain the same.  Note that I have removed the winforms code, since you'll still be able to edit queues &etc using System.Messaging; MSMQ Messaging is intended to upgrade your old code, not provide a management interface to create/edit queues. (You can still create queues with code, only the dialog boxes were removed.)

## Build and Test

Should be a simple clone and build, I have been using Visual Studio 2019.

## Contribute

Documentation needs to be updated, and unit tests need to be implemented.  Pull requests gratefully accepted.
