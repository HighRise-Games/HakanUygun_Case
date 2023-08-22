#if !DISABLE_SRDEBUGGER
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RubyGames
{
    public class SRDebuggerInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RunTimeInitialize()
        {
#if DEV_MODE
            SRDebug.Init();
            SRDebug.Instance.AddOptionContainer(GameSROptions.Instance);
#if RUBY_FRAMEWORK
            SRDebug.Instance.AddOptionContainer(RubyFrameworkSROptions.Instance);
#endif
#endif
        }
    }
}
#endif