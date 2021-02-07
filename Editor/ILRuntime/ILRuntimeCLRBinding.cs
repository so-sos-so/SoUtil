﻿#if UNITY_EDITOR
using UnityEditor;

namespace Framework.Editor
{
    [System.Reflection.Obfuscation(Exclude = true)]
    public class ILRuntimeCLRBinding
    {
        [MenuItem("Framework/ILRuntime/通过自动分析热更DLL生成CLR绑定")]
        static void GenerateCLRBindingByAnalysis()
        {
            //用新的分析热更dll调用引用来生成绑定代码
            ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
            using (System.IO.FileStream fs = new System.IO.FileStream("Assets/StreamingAssets/HotFix_Project.dll",
                System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                domain.LoadAssembly(fs);

                //Crossbind Adapter is needed to generate the correct binding code
                InitILRuntime(domain);
                ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain,
                    "Assets/Samples/ILRuntime/Generated");
            }

            AssetDatabase.Refresh();
        }

        static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain domain)
        {
            //这里需要注册所有热更DLL中用到的跨域继承Adapter，否则无法正确抓取引用
            ILRuntimeValueTypeBinderHelper.Register(domain);
            ILRuntimeAdapterHelper.RegisterCrossBindingAdaptor(domain);
        }
    }
#endif
}