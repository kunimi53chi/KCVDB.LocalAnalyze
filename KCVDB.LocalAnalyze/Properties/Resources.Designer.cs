﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace KCVDB.LocalAnalyze.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KCVDB.LocalAnalyze.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   インデックス	サーバーID	サーバー名	IPアドレス	バージョン	簡易サーバー名	区分	実装日	移転日
        ///1	1	横須賀鎮守府	203.104.105.167	0	横須賀	鎮守府	2013/4/23	
        ///2	2	呉鎮守府	125.6.184.15	0	呉	鎮守府	2013/7/10	
        ///3	2	呉鎮守府	203.104.209.87	1	呉	鎮守府	2013/7/10	2016/2/12
        ///4	3	佐世保鎮守府	125.6.184.16	0	佐世保	鎮守府	2013/7/10	
        ///5	4	舞鶴鎮守府	125.6.187.205	0	舞鶴	鎮守府	2013/7/17	
        ///6	5	大湊警備府	125.6.187.229	0	大湊	警備府	2013/8/6	
        ///7	6	トラック泊地	125.6.187.253	0	トラック	泊地	2013/8/10	
        ///8	7	リンガ泊地	125.6.188.25	0	リンガ	泊地	2013/8/17	
        ///9	8	ラバウル基地	203.104.248.135	0	ラバウル	基地	2013/8/21	
        ///10	9	ショートランド泊地	125.6.189.7	0	ショートランド	泊地	 [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string KCServerList {
            get {
                return ResourceManager.GetString("KCServerList", resourceCulture);
            }
        }
    }
}
