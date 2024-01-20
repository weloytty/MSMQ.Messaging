# What is MSMQ.Messaging

MSMQ.Messaging is intended to be a straight port of the .NET Framework's System.Messaging assembly to .NET Core, based on the [reference source](https://github.com/microsoft/referencesource).  Since MSMQ is platform specific, it will only run on Windows, and should work with .NET Standard 1.6 or .NET Framework 4.6.1 and up.  I originally created this project so that I could move legacy code to .NET Core, and it has only been tested with relatively simple use-cases.  

I'm not doing development on MSMQ.Messaging, it's only here so I can use .NET Core on some legacy applications.   

## Getting Started

You can use this from [NuGet](https://www.nuget.org/packages/MSMQ.Messaging/), or build directly.  Requires .NET Standard 1.6 or .NET Framework 4.6.1, and will run on Windows only.

It should be pretty straightforward: clone the source and dotnet build it. There's also a .sln file for use with Visual Studio 2019.  

1. MSMQ.Messaging relies on mqrt.dll to do the work, so while it will run on .NET Core, it will only do so on Windows.
2. All but the installation *should* work at this point, but test coverage is limited right now.
3. Configuring queues may or may not work, I don't know -- use cases I have don't require them.
4. API references -- [System.Messaging docs](https://docs.microsoft.com/en-us/dotnet/api/system.messaging?view=netframework-4.8) for classic .NET Framework should work for you, I have no intention of adding/changing any behavior.  I'm not an MSMQ person, I just want to be able to move some code that relies on MSMQ from the .NET Framework to .NET Core.  

## Upgrading .NET Framework projects

MSMQ.Messaging is intended to be simple to use if you are replacing System.Messaging code.  You should only have to change the references and change System.Messaging to MSMQ.Messaging in your usings section.  Everything else should remain the same.  Note that I have removed the winforms code, since you'll still be able to edit queues &etc using System.Messaging; MSMQ Messaging is intended to upgrade your old code, not provide a management interface to create/edit queues. (You can still create queues with code, only the dialog boxes were removed.)

## Build and Test

Should be a simple clone and build, I have been using Visual Studio 2019.

## Contribute

I am overjoyed that people are using this, but I am intentionally leaving it as close to the reference source as possible.  If you want to do something cool, please fork the code and have at it, but I don't expect to update this repo unless it stops working.
