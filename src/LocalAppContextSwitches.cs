// <copyright>
// Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>

using System;
using System.Runtime.CompilerServices;

namespace MSMQ
{
    

    internal static class LocalAppContextSwitches
    {
        private static int useMD5ForDefaultHashAlgorithm;

        public static bool UseMD5ForDefaultHashAlgorithm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return LocalAppContext.GetCachedSwitchValue(AppContextDefaultValues.UseMD5ForDefaultHashAlgorithmSwitchString, ref useMD5ForDefaultHashAlgorithm);
            }
        }
    }
}