# MSMQ.Messaging
A drop in replacement for System.Messaging for .netcore (Windows only)
# Introduction 
This is intended to be a drop-in replacement for System.Messaging, usable from .net core 3.1+.  Source will be added after https://github.com/microsoft/referencesource/pull/141 is merged into master for the reference source

# Getting Started
For now, you have to build from source, I've tested using Visual Studio 2019.  
1.	It relies on mqrt.dll to do the work, so while it will run on .netcore, it will only do so on Windows.
2.	All but the installation *should* work at this point, but test coverage is limited right now.
3.	Configuring queues may or may not work, I don't know -- use cases I have don't require them.
4.	API references -- [System.Messaging docs](https://docs.microsoft.com/en-us/dotnet/api/system.messaging?view=netframework-4.8) for classic .net framework should work for you, I have no intention of adding/changing any behavior.  I'm not an MSMQ person, I just want to be able to move some code that relies on MSMQ from the .NET Framework to .Net Core.  

# Build and Test
Should be a simple clone and build, I have been using Visual Studio 2019.

# Contribute
Documentation needs to be updated, and unit tests need to be implemented.  Pull requests gratefully accepted.
