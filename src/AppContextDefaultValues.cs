// <copyright>
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

using System;

namespace MSMQ
{
    internal static partial class AppContextDefaultValues
    {
        internal const string UseMD5ForDefaultHashAlgorithmSwitchString = @"Switch.System.Messaging.UseMD5ForDefaultHashAlgorithm";
        public static void PopulateDefaultValues()
        {

            //Since we're on dotnet core, none of this matters.

            //AppContextDefaultValues.ParseTargetFrameworkName(out var identifier, out var profile, out var version);
            //AppContextDefaultValues.PopulateDefaultValuesPartial(identifier, profile, version);
        }

        static  void PopulateDefaultValuesPartial(string platformIdentifier, string profile, int version)
        {
            switch (platformIdentifier)
            {
                case ".NETFramework":
                    {
                        // All previous versions of that platform (up-to 4.7) will get the old behavior by default 
                        if (version <= 40700)
                        {
                            LocalAppContext.DefineSwitchDefault(UseMD5ForDefaultHashAlgorithmSwitchString, true);
                        }

                        break;
                    }
            }
        }
    }
}
