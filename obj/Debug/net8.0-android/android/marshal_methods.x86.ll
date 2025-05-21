; ModuleID = 'marshal_methods.x86.ll'
source_filename = "marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [466 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [926 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 10895943, ; 2: de/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0xa64247 => 331
	i32 15721112, ; 3: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 4: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 251
	i32 34715100, ; 5: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 285
	i32 34839235, ; 6: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39109920, ; 7: Newtonsoft.Json.dll => 0x254c520 => 204
	i32 39485524, ; 8: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 9: System.Threading.Thread => 0x28aa24d => 145
	i32 45848530, ; 10: Microsoft.VisualStudio.TestPlatform.TestFramework.Extensions => 0x2bb97d2 => 460
	i32 66541672, ; 11: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 12: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 329
	i32 68219467, ; 13: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 14: Microsoft.Maui.Graphics.dll => 0x44bb714 => 190
	i32 82292897, ; 15: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 16: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 269
	i32 102064928, ; 17: zh-Hans\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x6156320 => 406
	i32 117431740, ; 18: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 19: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 269
	i32 122350210, ; 20: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 127272930, ; 21: Microsoft.Testing.Extensions.MSBuild.dll => 0x79607e2 => 195
	i32 134690465, ; 22: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 289
	i32 142721839, ; 23: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 24: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 25: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 26: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 225
	i32 165709020, ; 27: it\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x9e084dc => 399
	i32 167361067, ; 28: it\Microsoft.Testing.Extensions.MSBuild.resources => 0x9f9ba2b => 373
	i32 176265551, ; 29: System.ServiceProcess => 0xa81994f => 132
	i32 180640173, ; 30: ko\Microsoft.Testing.Extensions.Telemetry.resources => 0xac459ad => 336
	i32 182336117, ; 31: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 271
	i32 184328833, ; 32: System.ValueTuple.dll => 0xafca281 => 151
	i32 192212161, ; 33: pt-BR/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0xb74ecc1 => 403
	i32 195452805, ; 34: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 326
	i32 199333315, ; 35: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 327
	i32 203442366, ; 36: pt-BR/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0xc2048be => 390
	i32 205061960, ; 37: System.ComponentModel => 0xc38ff48 => 18
	i32 206219802, ; 38: ru\Microsoft.TestPlatform.CrossPlatEngine.resources => 0xc4aaa1a => 430
	i32 209399409, ; 39: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 223
	i32 211099368, ; 40: cs/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0xc951ee8 => 434
	i32 220171995, ; 41: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230216969, ; 42: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 245
	i32 230752869, ; 43: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 44: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 45: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 46: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 47: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 228
	i32 276479776, ; 48: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 276733289, ; 49: ko\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x107e9d69 => 349
	i32 278686392, ; 50: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 247
	i32 280482487, ; 51: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 244
	i32 280992041, ; 52: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 298
	i32 286582269, ; 53: ja\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x1114e5fd => 413
	i32 291076382, ; 54: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 55: System.Net.Ping.dll => 0x11d123fd => 69
	i32 300175404, ; 56: it/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x11e4502c => 399
	i32 305108063, ; 57: cs\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x122f945f => 395
	i32 312243509, ; 58: es\Microsoft.TestPlatform.CoreUtilities.resources => 0x129c7535 => 384
	i32 317674968, ; 59: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 326
	i32 318968648, ; 60: Xamarin.AndroidX.Activity.dll => 0x13031348 => 214
	i32 319144191, ; 61: ru/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0x1305c0ff => 430
	i32 321597661, ; 62: System.Numerics => 0x132b30dd => 83
	i32 324822548, ; 63: ko/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x135c6614 => 453
	i32 336156722, ; 64: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 311
	i32 336681816, ; 65: zh-Hant\Microsoft.Testing.Extensions.Telemetry.resources => 0x14115b58 => 342
	i32 342366114, ; 66: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 246
	i32 347068432, ; 67: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 208
	i32 351129079, ; 68: pt-BR\Microsoft.Testing.Extensions.MSBuild.resources => 0x14edcdf7 => 377
	i32 356389973, ; 69: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 310
	i32 360082299, ; 70: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 71: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 72: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 73: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 74: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 75: System.Memory.dll => 0x16fe439a => 62
	i32 389482135, ; 76: tr\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x17370697 => 431
	i32 392610295, ; 77: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 78: _Microsoft.Android.Resource.Designer => 0x17969339 => 461
	i32 402606684, ; 79: ja/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x17ff4a5c => 413
	i32 403441872, ; 80: WindowsBase => 0x180c08d0 => 165
	i32 405657606, ; 81: zh-Hant\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x182dd806 => 407
	i32 409916085, ; 82: ko\Microsoft.Testing.Platform.resources => 0x186ed2b5 => 362
	i32 425078024, ; 83: es\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x19562d08 => 449
	i32 434296532, ; 84: cs\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x19e2d6d4 => 343
	i32 435591531, ; 85: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 322
	i32 439864463, ; 86: ja\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x1a37cc8f => 426
	i32 441335492, ; 87: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 229
	i32 442565967, ; 88: System.Collections => 0x1a61054f => 12
	i32 444359321, ; 89: de/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x1a7c6299 => 435
	i32 450948140, ; 90: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 242
	i32 451504562, ; 91: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 92: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 457775588, ; 93: pl/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x1b4919e4 => 389
	i32 459347974, ; 94: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 95: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 96: System.dll => 0x1bff388e => 164
	i32 476646585, ; 97: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 244
	i32 486930444, ; 98: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 257
	i32 498788369, ; 99: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 100: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 309
	i32 503918385, ; 101: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 303
	i32 508376876, ; 102: Microsoft.VisualStudio.TestPlatform.Common => 0x1e4d372c => 202
	i32 513247710, ; 103: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 184
	i32 518856540, ; 104: it\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x1eed1f5c => 347
	i32 522289150, ; 105: ja/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x1f217ffe => 387
	i32 526420162, ; 106: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 107: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 289
	i32 530272170, ; 108: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 109: Microsoft.Extensions.Logging => 0x20216150 => 180
	i32 540030774, ; 110: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 111: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 112: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549153152, ; 113: Microsoft.VisualStudio.TestPlatform.TestFramework.Extensions.dll => 0x20bb6980 => 460
	i32 549171840, ; 114: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 550200776, ; 115: ja/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x20cb65c8 => 452
	i32 557405415, ; 116: Jsr305Binding => 0x213954e7 => 282
	i32 569601784, ; 117: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 280
	i32 577298002, ; 118: Microsoft.VisualStudio.TestPlatform.Common.dll => 0x2268de52 => 202
	i32 577335427, ; 119: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 579873359, ; 120: zh-Hant\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x22902a4f => 433
	i32 588378213, ; 121: zh-Hant/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x2311f065 => 394
	i32 592146354, ; 122: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 317
	i32 599687487, ; 123: Microsoft.TestPlatform.Utilities => 0x23be813f => 201
	i32 601371474, ; 124: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 604296586, ; 125: ko/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x2404d58a => 388
	i32 605376203, ; 126: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 127: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 616099426, ; 128: zh-Hans\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x24b8ee62 => 458
	i32 627609679, ; 129: Xamarin.AndroidX.CustomView => 0x2568904f => 234
	i32 627931235, ; 130: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 315
	i32 639843206, ; 131: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 240
	i32 643078058, ; 132: Microsoft.ApplicationInsights.dll => 0x265497aa => 174
	i32 643868501, ; 133: System.Net => 0x2660a755 => 81
	i32 656670485, ; 134: Microsoft.TestPlatform.PlatformAbstractions.dll => 0x2723ff15 => 197
	i32 662205335, ; 135: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663245147, ; 136: Microsoft.Testing.Extensions.VSTestBridge => 0x2788515b => 193
	i32 663517072, ; 137: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 276
	i32 666292255, ; 138: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 221
	i32 670496576, ; 139: ja\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x27f6f740 => 439
	i32 672442732, ; 140: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 141: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 142: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 297
	i32 690569205, ; 143: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 144: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 291
	i32 693804605, ; 145: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 146: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 147: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 286
	i32 700358131, ; 148: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 149: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 312
	i32 706771697, ; 150: pt-BR\Microsoft.Testing.Extensions.Telemetry.resources => 0x2a207af1 => 338
	i32 709557578, ; 151: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 300
	i32 720511267, ; 152: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 290
	i32 722857257, ; 153: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 154: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 748832960, ; 155: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 206
	i32 750365387, ; 156: xunit.core.dll => 0x2cb9aacb => 294
	i32 751361846, ; 157: it\Microsoft.Testing.Platform.resources => 0x2cc8df36 => 360
	i32 752232764, ; 158: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 159: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 211
	i32 756716739, ; 160: it\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x2d1a94c3 => 451
	i32 759454413, ; 161: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 162: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 163: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 164: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 321
	i32 788959164, ; 165: ko\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x2f068fbc => 427
	i32 789151979, ; 166: Microsoft.Extensions.Options => 0x2f0980eb => 183
	i32 790371945, ; 167: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 235
	i32 795583798, ; 168: Microsoft.TestPlatform.CommunicationUtilities.dll => 0x2f6ba536 => 199
	i32 798358160, ; 169: Microsoft.VisualStudio.CodeCoverage.Shim => 0x2f95fa90 => 175
	i32 799585402, ; 170: fr/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x2fa8b47a => 346
	i32 804566382, ; 171: pl/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x2ff4b56e => 402
	i32 804715423, ; 172: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 173: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 249
	i32 809136923, ; 174: pl/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x303a731b => 337
	i32 818048070, ; 175: cs\Microsoft.TestPlatform.CoreUtilities.resources => 0x30c26c46 => 382
	i32 823117470, ; 176: zh-Hans/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x310fc69e => 458
	i32 823281589, ; 177: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 178: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 179: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 180: System.Net.Quic => 0x31b69d60 => 71
	i32 836438132, ; 181: fr\Microsoft.Testing.Extensions.MSBuild.resources => 0x31db0874 => 372
	i32 843511501, ; 182: Xamarin.AndroidX.Print => 0x3246f6cd => 262
	i32 856402831, ; 183: pt-BR/Microsoft.Testing.Platform.resources.dll => 0x330bab8f => 364
	i32 856640110, ; 184: es/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x330f4a6e => 436
	i32 857506242, ; 185: Microsoft.VisualStudio.TestPlatform.ObjectModel.dll => 0x331c81c2 => 198
	i32 873119928, ; 186: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 187: System.Globalization.dll => 0x34505120 => 42
	i32 878167261, ; 188: it/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x3457c4dd => 334
	i32 878954865, ; 189: System.Net.Http.Json => 0x3463c971 => 63
	i32 885081167, ; 190: pt-BR/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x34c1444f => 442
	i32 903397943, ; 191: pl/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0x35d8c237 => 428
	i32 904024072, ; 192: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 908615579, ; 193: fr/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x36285f9b => 437
	i32 909689493, ; 194: zh-Hant\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x3638c295 => 420
	i32 911108515, ; 195: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 926902833, ; 196: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 324
	i32 928116545, ; 197: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 285
	i32 938387646, ; 198: cs\Microsoft.Testing.Extensions.MSBuild.resources => 0x37eea8be => 369
	i32 944026163, ; 199: zh-Hant/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x3844b233 => 355
	i32 952186615, ; 200: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 955402788, ; 201: Newtonsoft.Json => 0x38f24a24 => 204
	i32 956575887, ; 202: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 290
	i32 957783329, ; 203: tr/Microsoft.Testing.Platform.resources.dll => 0x39169d21 => 366
	i32 966729478, ; 204: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 283
	i32 967690846, ; 205: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 246
	i32 974162455, ; 206: ko/Microsoft.Testing.Platform.resources.dll => 0x3a108a17 => 362
	i32 975236339, ; 207: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 208: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 983135203, ; 209: zh-Hant/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x3a9973e3 => 459
	i32 986514023, ; 210: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 211: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 212: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 213: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 998234664, ; 214: pt-BR\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x3b7fda28 => 351
	i32 1001831731, ; 215: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 216: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 266
	i32 1018167263, ; 217: pl/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x3cafffdf => 441
	i32 1019214401, ; 218: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 219: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 179
	i32 1029334545, ; 220: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 299
	i32 1031528504, ; 221: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 284
	i32 1035115987, ; 222: fr/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x3db29dd3 => 411
	i32 1035644815, ; 223: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 219
	i32 1036536393, ; 224: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 225: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 226: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 253
	i32 1052927897, ; 227: de/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x3ec26799 => 370
	i32 1055662153, ; 228: tr\Microsoft.Testing.Extensions.Telemetry.resources => 0x3eec2049 => 340
	i32 1067306892, ; 229: GoogleGson => 0x3f9dcf8c => 173
	i32 1070982722, ; 230: fr/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x3fd5e642 => 333
	i32 1082857460, ; 231: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1083637766, ; 232: zh-Hant\Microsoft.TestPlatform.CoreUtilities.resources => 0x40970006 => 394
	i32 1084122840, ; 233: Xamarin.Kotlin.StdLib => 0x409e66d8 => 287
	i32 1096242785, ; 234: fr/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x41575661 => 398
	i32 1098259244, ; 235: System => 0x41761b2c => 164
	i32 1100337094, ; 236: ko/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x4195cfc6 => 401
	i32 1100926387, ; 237: fr\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x419ecdb3 => 450
	i32 1109435305, ; 238: xunit.execution.dotnet.dll => 0x4220a3a9 => 295
	i32 1112489028, ; 239: ko/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x424f3c44 => 375
	i32 1114216289, ; 240: zh-Hant\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x42699761 => 459
	i32 1116596734, ; 241: tr\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x428de9fe => 353
	i32 1118262833, ; 242: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 312
	i32 1121599056, ; 243: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 252
	i32 1127624469, ; 244: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 182
	i32 1149092582, ; 245: Xamarin.AndroidX.Window => 0x447dc2e6 => 279
	i32 1155750561, ; 246: de\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x44e35aa1 => 422
	i32 1163081802, ; 247: it/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x4553384a => 373
	i32 1165181021, ; 248: tr\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x4573405d => 418
	i32 1168523401, ; 249: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 318
	i32 1170634674, ; 250: System.Web.dll => 0x45c677b2 => 153
	i32 1175002586, ; 251: cs\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x46091dda => 408
	i32 1175144683, ; 252: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 275
	i32 1178241025, ; 253: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 260
	i32 1182671637, ; 254: cs/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x467e2315 => 382
	i32 1185835922, ; 255: tr\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x46ae6b92 => 405
	i32 1203215381, ; 256: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 316
	i32 1204270330, ; 257: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 221
	i32 1208641965, ; 258: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1211243070, ; 259: xunit.assert => 0x48321a3e => 293
	i32 1217262239, ; 260: ru/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x488df29f => 352
	i32 1219128291, ; 261: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1219695198, ; 262: Microsoft.Testing.Platform.dll => 0x48b3125e => 194
	i32 1221074211, ; 263: ru\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x48c81d23 => 456
	i32 1224973627, ; 264: zh-Hant/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x49039d3b => 407
	i32 1234928153, ; 265: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 314
	i32 1235611439, ; 266: pt-BR/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x49a5ef2f => 377
	i32 1240933164, ; 267: it/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0x49f7232c => 425
	i32 1243150071, ; 268: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 280
	i32 1253011324, ; 269: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 270: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 298
	i32 1262228757, ; 271: zh-Hans\Microsoft.Testing.Extensions.Telemetry.resources => 0x4b3c1515 => 341
	i32 1262772391, ; 272: zh-Hans/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x4b4460a7 => 406
	i32 1264511973, ; 273: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 270
	i32 1267360935, ; 274: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 274
	i32 1273260888, ; 275: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 226
	i32 1275534314, ; 276: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 291
	i32 1278448581, ; 277: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 218
	i32 1279056436, ; 278: ru\Microsoft.Testing.Extensions.MSBuild.resources => 0x4c3cda34 => 378
	i32 1287326769, ; 279: ko\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x4cbb0c31 => 414
	i32 1287719757, ; 280: xunit.execution.dotnet => 0x4cc10b4d => 295
	i32 1289200857, ; 281: Microsoft.TestPlatform.PlatformAbstractions => 0x4cd7a4d9 => 197
	i32 1290915758, ; 282: ja/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x4cf1cfae => 335
	i32 1291411246, ; 283: tr\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x4cf95f2e => 444
	i32 1292207520, ; 284: SQLitePCLRaw.core.dll => 0x4d0585a0 => 207
	i32 1293217323, ; 285: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 237
	i32 1296723087, ; 286: ko/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x4d4a6c8f => 349
	i32 1309188875, ; 287: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1313559064, ; 288: cs\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x4e4b5218 => 447
	i32 1318484685, ; 289: pl\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x4e967acd => 350
	i32 1319053945, ; 290: pt-BR/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x4e9f2a79 => 338
	i32 1322716291, ; 291: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 279
	i32 1324164729, ; 292: System.Linq => 0x4eed2679 => 61
	i32 1329097642, ; 293: it\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x4f386baa => 438
	i32 1335329327, ; 294: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1358489281, ; 295: Microsoft.Testing.Extensions.MSBuild => 0x50f8e6c1 => 195
	i32 1364015309, ; 296: System.IO => 0x514d38cd => 57
	i32 1365712339, ; 297: pl\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x51671dd3 => 441
	i32 1373134921, ; 298: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 328
	i32 1376866003, ; 299: Xamarin.AndroidX.SavedState => 0x52114ed3 => 266
	i32 1379779777, ; 300: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 301: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 302: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 230
	i32 1407915212, ; 303: de/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x53eb14cc => 344
	i32 1408764838, ; 304: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1410695656, ; 305: ja\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x541581e8 => 348
	i32 1411638395, ; 306: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1416038589, ; 307: pl/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x546708bd => 454
	i32 1422545099, ; 308: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1424483224, ; 309: ja/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x54e7e398 => 348
	i32 1426853131, ; 310: pt-BR/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x550c0d0b => 455
	i32 1430672901, ; 311: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 296
	i32 1434145427, ; 312: System.Runtime.Handles => 0x557b5293 => 104
	i32 1434218365, ; 313: pt-BR\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x557c6f7d => 455
	i32 1435222561, ; 314: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 283
	i32 1439761251, ; 315: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1448553051, ; 316: tr/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x56572a5b => 340
	i32 1452070440, ; 317: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 318: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 319: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 320: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 321: es\Microsoft.Maui.Controls.resources => 0x57152abe => 302
	i32 1461234159, ; 322: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 323: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1461797484, ; 324: pt-BR\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x5721426c => 403
	i32 1462112819, ; 325: System.IO.Compression.dll => 0x57261233 => 46
	i32 1463986084, ; 326: zh-Hant/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x5742a7a4 => 446
	i32 1469204771, ; 327: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 220
	i32 1470490898, ; 328: Microsoft.Extensions.Primitives => 0x57a5e912 => 184
	i32 1479771757, ; 329: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 330: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 331: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 332: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 267
	i32 1493001747, ; 333: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 306
	i32 1509121692, ; 334: Zettelkasten => 0x59f35e9c => 0
	i32 1509633027, ; 335: de\Microsoft.Testing.Extensions.Telemetry.resources => 0x59fb2c03 => 331
	i32 1514721132, ; 336: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 301
	i32 1536373174, ; 337: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1537378065, ; 338: ru/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x5ba28711 => 339
	i32 1543031311, ; 339: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 340: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 341: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 342: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 321
	i32 1561818847, ; 343: zh-Hant/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0x5d1776df => 433
	i32 1565693220, ; 344: pl\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x5d529524 => 415
	i32 1565862583, ; 345: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 346: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 347: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 348: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 349: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 236
	i32 1585295981, ; 350: es/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x5e7db26d => 410
	i32 1586683303, ; 351: Microsoft.TestPlatform.CrossPlatEngine.dll => 0x5e92dda7 => 200
	i32 1592978981, ; 352: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1593685786, ; 353: de/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x5efdb71a => 409
	i32 1597949149, ; 354: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 284
	i32 1601112923, ; 355: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 356: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1615200841, ; 357: pt-BR/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x60460249 => 351
	i32 1618516317, ; 358: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 359: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 256
	i32 1622358360, ; 360: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 361: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 278
	i32 1635184631, ; 362: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 240
	i32 1636350590, ; 363: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 233
	i32 1639515021, ; 364: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 365: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 366: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 367: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 368: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 272
	i32 1658251792, ; 369: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 281
	i32 1670060433, ; 370: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 228
	i32 1670549604, ; 371: ja\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x63929064 => 400
	i32 1675553242, ; 372: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 373: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 374: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 375: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 376: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696485959, ; 377: de\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x651e5247 => 396
	i32 1696505114, ; 378: it/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x651e9d1a => 451
	i32 1696967625, ; 379: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 380: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 288
	i32 1701541528, ; 381: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1703516718, ; 382: ru\Microsoft.TestPlatform.CoreUtilities.resources => 0x65899a2e => 391
	i32 1711441057, ; 383: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 208
	i32 1720223769, ; 384: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 249
	i32 1726116996, ; 385: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 386: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 387: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 224
	i32 1736233607, ; 388: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 319
	i32 1743415430, ; 389: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 297
	i32 1744735666, ; 390: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746316138, ; 391: Mono.Android.Export => 0x6816ab6a => 169
	i32 1749479404, ; 392: cs/Microsoft.Testing.Platform.resources.dll => 0x6846efec => 356
	i32 1750313021, ; 393: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 394: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 395: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765742778, ; 396: ja/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x693f18ba => 374
	i32 1765942094, ; 397: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 398: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 271
	i32 1770582343, ; 399: Microsoft.Extensions.Logging.dll => 0x6988f147 => 180
	i32 1773394522, ; 400: es\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x69b3da5a => 423
	i32 1776026572, ; 401: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 402: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 403: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 404: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 313
	i32 1788241197, ; 405: Xamarin.AndroidX.Fragment => 0x6a96652d => 242
	i32 1791986079, ; 406: pl\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x6acf899f => 428
	i32 1793755602, ; 407: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 305
	i32 1806265311, ; 408: zh-Hans\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x6ba96bdf => 354
	i32 1807600182, ; 409: de\Microsoft.Testing.Extensions.MSBuild.resources => 0x6bbdca36 => 370
	i32 1808609942, ; 410: Xamarin.AndroidX.Loader => 0x6bcd3296 => 256
	i32 1811883999, ; 411: fr\Microsoft.TestPlatform.CoreUtilities.resources => 0x6bff27df => 385
	i32 1813058853, ; 412: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 287
	i32 1813201214, ; 413: Xamarin.Google.Android.Material => 0x6c13413e => 281
	i32 1818569960, ; 414: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 261
	i32 1818787751, ; 415: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 416: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 417: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1826012542, ; 418: es/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x6cd6bd7e => 345
	i32 1828688058, ; 419: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 181
	i32 1842015223, ; 420: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 325
	i32 1847515442, ; 421: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 211
	i32 1850379862, ; 422: Microsoft.ApplicationInsights => 0x6e4a8e56 => 174
	i32 1853025655, ; 423: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 322
	i32 1858542181, ; 424: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 425: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 426: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 304
	i32 1878664241, ; 427: pl\Microsoft.Testing.Platform.resources => 0x6ffa2431 => 363
	i32 1879696579, ; 428: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1881903010, ; 429: pl\Microsoft.TestPlatform.CoreUtilities.resources => 0x702b8fa2 => 389
	i32 1881913493, ; 430: xunit.assert.dll => 0x702bb895 => 293
	i32 1885316902, ; 431: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 222
	i32 1888955245, ; 432: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 433: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 434: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 435: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 436: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1915695524, ; 437: it\Microsoft.TestPlatform.CoreUtilities.resources => 0x722f31a4 => 386
	i32 1923188307, ; 438: it/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x72a18653 => 347
	i32 1926819048, ; 439: de\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x72d8ece8 => 448
	i32 1939592360, ; 440: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1945827410, ; 441: Microsoft.VisualStudio.TestPlatform.TestFramework.dll => 0x73faf852 => 203
	i32 1956758971, ; 442: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1957100751, ; 443: zh-Hans/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x74a6fccf => 380
	i32 1961813231, ; 444: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 268
	i32 1968388702, ; 445: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 176
	i32 1983156543, ; 446: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 288
	i32 1985761444, ; 447: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 213
	i32 2001249003, ; 448: tr/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x7748a2eb => 444
	i32 2003115576, ; 449: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 301
	i32 2011961780, ; 450: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2013676115, ; 451: cs/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0x78064253 => 343
	i32 2019465201, ; 452: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 253
	i32 2020687097, ; 453: pl/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x78713cf9 => 415
	i32 2025202353, ; 454: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 296
	i32 2031763787, ; 455: Xamarin.Android.Glide => 0x791a414b => 210
	i32 2035888002, ; 456: ko\Microsoft.Testing.Extensions.MSBuild.resources => 0x79592f82 => 375
	i32 2045470958, ; 457: System.Private.Xml => 0x79eb68ee => 88
	i32 2045667571, ; 458: de/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x79ee68f3 => 396
	i32 2055257422, ; 459: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 248
	i32 2058141057, ; 460: ko/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x7aacbd81 => 414
	i32 2060060697, ; 461: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 462: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 300
	i32 2070888862, ; 463: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 464: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 465: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2103459038, ; 466: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 209
	i32 2127167465, ; 467: System.Console => 0x7ec9ffe9 => 20
	i32 2128581131, ; 468: pt-BR/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x7edf920b => 416
	i32 2142473426, ; 469: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 470: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 471: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2153351447, ; 472: Zettelkasten.dll => 0x80598917 => 0
	i32 2159891885, ; 473: Microsoft.Maui => 0x80bd55ad => 188
	i32 2166284992, ; 474: pt-BR/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0x811ee2c0 => 429
	i32 2169148018, ; 475: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 308
	i32 2172808666, ; 476: pt-BR\Microsoft.Testing.Platform.resources => 0x81826dda => 364
	i32 2181898931, ; 477: Microsoft.Extensions.Options.dll => 0x820d22b3 => 183
	i32 2192057212, ; 478: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 181
	i32 2193016926, ; 479: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 480: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 292
	i32 2201231467, ; 481: System.Net.Http => 0x8334206b => 64
	i32 2205756526, ; 482: es\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x83792c6e => 345
	i32 2207618523, ; 483: it\Microsoft.Maui.Controls.resources => 0x839595db => 310
	i32 2217644978, ; 484: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 275
	i32 2222056684, ; 485: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2233257987, ; 486: pl/Microsoft.Testing.Platform.resources.dll => 0x851cd003 => 363
	i32 2244775296, ; 487: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 257
	i32 2252106437, ; 488: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2252377297, ; 489: ko/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0x86408cd1 => 427
	i32 2256313426, ; 490: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 491: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2265723706, ; 492: es/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x870c333a => 449
	i32 2266799131, ; 493: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 177
	i32 2267999099, ; 494: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 212
	i32 2270026463, ; 495: fr/Microsoft.Testing.Platform.resources.dll => 0x874ddadf => 359
	i32 2270573516, ; 496: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 304
	i32 2279755925, ; 497: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 264
	i32 2293034957, ; 498: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 499: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2296684434, ; 500: zh-Hant\Microsoft.Testing.Extensions.MSBuild.resources => 0x88e49f92 => 381
	i32 2298471582, ; 501: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 502: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 314
	i32 2305165409, ; 503: tr/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x89660861 => 457
	i32 2305521784, ; 504: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2305641583, ; 505: Microsoft.TestPlatform.CoreUtilities => 0x896d4c6f => 196
	i32 2313216723, ; 506: pl\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x89e0e2d3 => 454
	i32 2315684594, ; 507: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 216
	i32 2320631194, ; 508: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2329792788, ; 509: zh-Hans/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x8addd114 => 341
	i32 2340441535, ; 510: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 511: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2348814802, ; 512: cs\Microsoft.Testing.Extensions.Telemetry.resources => 0x8c0011d2 => 330
	i32 2353062107, ; 513: System.Net.Primitives => 0x8c40e0db => 70
	i32 2358713569, ; 514: de/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x8c971ce1 => 448
	i32 2359133437, ; 515: cs/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0x8c9d84fd => 447
	i32 2363036410, ; 516: ru\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0x8cd912fa => 404
	i32 2366988767, ; 517: cs\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x8d1561df => 421
	i32 2368005991, ; 518: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2371007202, ; 519: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 176
	i32 2378619854, ; 520: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 521: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 522: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 309
	i32 2401565422, ; 523: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 524: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 239
	i32 2421380589, ; 525: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 526: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 226
	i32 2424089243, ; 527: ru/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0x907caa9b => 404
	i32 2427813419, ; 528: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 306
	i32 2435356389, ; 529: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 530: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2444332367, ; 531: it\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x91b18d4f => 412
	i32 2452384599, ; 532: zh-Hant\Microsoft.Testing.Platform.resources => 0x922c6b57 => 368
	i32 2454642406, ; 533: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 534: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 535: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465273461, ; 536: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 206
	i32 2465532216, ; 537: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 229
	i32 2471841756, ; 538: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 539: Java.Interop.dll => 0x93918882 => 168
	i32 2475823519, ; 540: zh-Hans/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x9392119f => 419
	i32 2478591493, ; 541: xunit.core => 0x93bc4e05 => 294
	i32 2480646305, ; 542: Microsoft.Maui.Controls => 0x93dba8a1 => 186
	i32 2481511830, ; 543: Microsoft.Testing.Extensions.TrxReport.Abstractions => 0x93e8dd96 => 192
	i32 2483375731, ; 544: ru\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x94054e73 => 443
	i32 2483903535, ; 545: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 546: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 547: System.AppContext.dll => 0x94798bc5 => 6
	i32 2491501833, ; 548: it\Microsoft.Testing.Extensions.Telemetry.resources => 0x94814d09 => 334
	i32 2496790010, ; 549: pt-BR\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x94d1fdfa => 442
	i32 2501346920, ; 550: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 551: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 251
	i32 2522472828, ; 552: Xamarin.Android.Glide.dll => 0x9659e17c => 210
	i32 2533828407, ; 553: de\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x97072737 => 344
	i32 2538310050, ; 554: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2547061934, ; 555: ru\Microsoft.Testing.Platform.resources => 0x97d114ae => 365
	i32 2550873716, ; 556: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 307
	i32 2562349572, ; 557: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2567786183, ; 558: fr/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x990d4ec7 => 385
	i32 2570120770, ; 559: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581783588, ; 560: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 252
	i32 2581819634, ; 561: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 274
	i32 2585220780, ; 562: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 563: System.Net.Ping => 0x9a20430d => 69
	i32 2589451624, ; 564: pt-BR\Microsoft.TestPlatform.CrossPlatEngine.resources => 0x9a57e568 => 429
	i32 2589602615, ; 565: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2590448731, ; 566: ja\Microsoft.Testing.Platform.resources => 0x9a671c5b => 361
	i32 2593496499, ; 567: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 316
	i32 2605712449, ; 568: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 292
	i32 2611875357, ; 569: testhost.dll => 0x9bae0e1d => 462
	i32 2613717964, ; 570: Microsoft.VisualStudio.TestPlatform.ObjectModel => 0x9bca2bcc => 198
	i32 2615233544, ; 571: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 243
	i32 2616218305, ; 572: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 182
	i32 2617129537, ; 573: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 574: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2619759289, ; 575: zh-Hant\Microsoft.Testing.Extensions.VSTestBridge.resources => 0x9c265ab9 => 355
	i32 2620871830, ; 576: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 233
	i32 2622582488, ; 577: es/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0x9c516ed8 => 332
	i32 2624644809, ; 578: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 238
	i32 2626831493, ; 579: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 311
	i32 2627185994, ; 580: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 581: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 582: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 247
	i32 2635167307, ; 583: cs/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0x9d11764b => 408
	i32 2635799312, ; 584: fr/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0x9d1b1b10 => 372
	i32 2636044767, ; 585: ja/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0x9d1ed9df => 439
	i32 2642966998, ; 586: ru\Microsoft.Testing.Extensions.Telemetry.resources => 0x9d8879d6 => 339
	i32 2648251943, ; 587: de/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0x9dd91e27 => 383
	i32 2652614677, ; 588: zh-Hant\Microsoft.VisualStudio.TestPlatform.Common.resources => 0x9e1bb015 => 446
	i32 2653966914, ; 589: de/Microsoft.Testing.Platform.resources.dll => 0x9e305242 => 357
	i32 2663391936, ; 590: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 212
	i32 2663698177, ; 591: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2663973326, ; 592: pt-BR\Microsoft.TestPlatform.CommunicationUtilities.resources => 0x9ec901ce => 416
	i32 2664396074, ; 593: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 594: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 595: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2681289175, ; 596: ko\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0x9fd139d7 => 453
	i32 2686887180, ; 597: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2688997888, ; 598: tr/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xa046da00 => 431
	i32 2693849962, ; 599: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 600: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 272
	i32 2705784343, ; 601: fr/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0xa146fe17 => 450
	i32 2715334215, ; 602: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 603: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719961531, ; 604: es\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0xa21f51bb => 397
	i32 2719963679, ; 605: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 606: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 607: Xamarin.AndroidX.Activity => 0xa2e0939b => 214
	i32 2735172069, ; 608: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 609: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 220
	i32 2740948882, ; 610: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 611: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 612: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 317
	i32 2758225723, ; 613: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 187
	i32 2764765095, ; 614: Microsoft.Maui.dll => 0xa4caf7a7 => 188
	i32 2765824710, ; 615: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 616: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 286
	i32 2778768386, ; 617: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 277
	i32 2779977773, ; 618: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 265
	i32 2785988530, ; 619: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 323
	i32 2788224221, ; 620: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 243
	i32 2801831435, ; 621: Microsoft.Maui.Graphics => 0xa7008e0b => 190
	i32 2803228030, ; 622: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2806116107, ; 623: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 302
	i32 2807542604, ; 624: it/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0xa757b34c => 412
	i32 2810250172, ; 625: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 230
	i32 2816058454, ; 626: fr/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xa7d9a456 => 424
	i32 2816910783, ; 627: pl/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0xa7e6a5bf => 376
	i32 2817380358, ; 628: testhost => 0xa7edd006 => 462
	i32 2819470561, ; 629: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 630: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 631: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 265
	i32 2824502124, ; 632: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2825246514, ; 633: Microsoft.VisualStudio.CodeCoverage.Shim.dll => 0xa865d732 => 175
	i32 2831556043, ; 634: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 315
	i32 2837859325, ; 635: ja/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xa9264bfd => 426
	i32 2838993487, ; 636: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 254
	i32 2849599387, ; 637: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853079587, ; 638: zh-Hans\Microsoft.TestPlatform.CoreUtilities.resources => 0xaa0e8a23 => 393
	i32 2853208004, ; 639: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 277
	i32 2855708567, ; 640: Xamarin.AndroidX.Transition => 0xaa36a797 => 273
	i32 2857977710, ; 641: zh-Hans\Microsoft.TestPlatform.CommunicationUtilities.resources => 0xaa59476e => 419
	i32 2858994200, ; 642: ru/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0xaa68ca18 => 391
	i32 2861098320, ; 643: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 644: Microsoft.Maui.Essentials => 0xaa8a4878 => 189
	i32 2867808321, ; 645: ja\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0xaaef4841 => 452
	i32 2870099610, ; 646: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 215
	i32 2875164099, ; 647: Jsr305Binding.dll => 0xab5f85c3 => 282
	i32 2875220617, ; 648: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2883439763, ; 649: zh-Hans\Microsoft.Testing.Extensions.MSBuild.resources => 0xabddcc93 => 380
	i32 2884993177, ; 650: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 241
	i32 2887636118, ; 651: System.Net.dll => 0xac1dd496 => 81
	i32 2888825240, ; 652: zh-Hant/Microsoft.Testing.Platform.resources.dll => 0xac2ff998 => 368
	i32 2892921710, ; 653: tr/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0xac6e7b6e => 379
	i32 2899753641, ; 654: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900425020, ; 655: cs/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0xace0f93c => 369
	i32 2900621748, ; 656: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 657: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 658: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 659: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2911788893, ; 660: zh-Hant/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0xad8e5f5d => 420
	i32 2916838712, ; 661: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 278
	i32 2919128806, ; 662: pl\Microsoft.Testing.Extensions.Telemetry.resources => 0xadfe5ee6 => 337
	i32 2919462931, ; 663: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2920932955, ; 664: zh-Hans/Microsoft.Testing.Platform.resources.dll => 0xae19e65b => 367
	i32 2921128767, ; 665: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 217
	i32 2934941515, ; 666: pt-BR\Microsoft.TestPlatform.CoreUtilities.resources => 0xaeefa74b => 390
	i32 2936416060, ; 667: System.Resources.Reader => 0xaf06273c => 98
	i32 2937137298, ; 668: ko\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0xaf112892 => 401
	i32 2940926066, ; 669: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 670: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2952067056, ; 671: zh-Hant/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0xaff4f7f0 => 381
	i32 2958555520, ; 672: ru/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0xb057f980 => 417
	i32 2959614098, ; 673: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2966790109, ; 674: zh-Hans/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xb0d59fdd => 432
	i32 2968338931, ; 675: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2970374337, ; 676: tr\Microsoft.Testing.Platform.resources => 0xb10c50c1 => 366
	i32 2972252294, ; 677: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 678: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 237
	i32 2987532451, ; 679: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 268
	i32 2996846495, ; 680: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 250
	i32 2999602259, ; 681: it/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0xb2ca4c53 => 386
	i32 3016983068, ; 682: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 270
	i32 3023353419, ; 683: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 684: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 245
	i32 3027982601, ; 685: es/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0xb47b5909 => 397
	i32 3038032645, ; 686: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 461
	i32 3045214156, ; 687: ko/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0xb58247cc => 336
	i32 3055454909, ; 688: fr\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0xb61e8abd => 398
	i32 3056245963, ; 689: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 267
	i32 3057625584, ; 690: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 258
	i32 3059408633, ; 691: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 692: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3066921912, ; 693: es/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0xb6cd83b8 => 384
	i32 3075834255, ; 694: System.Threading.Tasks => 0xb755818f => 144
	i32 3075886166, ; 695: tr/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0xb7564c56 => 392
	i32 3077302341, ; 696: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 308
	i32 3084080817, ; 697: zh-Hant/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0xb7d356b1 => 342
	i32 3088139648, ; 698: ja/Microsoft.Testing.Platform.resources.dll => 0xb8114580 => 361
	i32 3090735792, ; 699: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 700: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 701: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 702: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 703: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 704: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3129864195, ; 705: ru\Microsoft.Testing.Extensions.VSTestBridge.resources => 0xba8df003 => 352
	i32 3132293585, ; 706: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 707: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 708: GoogleGson.dll => 0xbba64c02 => 173
	i32 3149210077, ; 709: tr\Microsoft.Testing.Extensions.MSBuild.resources => 0xbbb521dd => 379
	i32 3152804951, ; 710: cs\Microsoft.VisualStudio.TestPlatform.Common.resources => 0xbbebfc57 => 434
	i32 3156928370, ; 711: es\Microsoft.Testing.Extensions.Telemetry.resources => 0xbc2ae772 => 332
	i32 3159123045, ; 712: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 713: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3177274847, ; 714: fr\Microsoft.Testing.Extensions.VSTestBridge.resources => 0xbd615ddf => 346
	i32 3178803400, ; 715: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 259
	i32 3185835721, ; 716: ru/Microsoft.Testing.Platform.resources.dll => 0xbde3fec9 => 365
	i32 3188718548, ; 717: ko\Microsoft.TestPlatform.CoreUtilities.resources => 0xbe0ffbd4 => 388
	i32 3192346100, ; 718: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 719: System.Web => 0xbe592c0c => 153
	i32 3204380047, ; 720: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 721: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 722: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 236
	i32 3214337465, ; 723: it\Microsoft.TestPlatform.CrossPlatEngine.resources => 0xbf96e5b9 => 425
	i32 3220365878, ; 724: System.Threading => 0xbff2e236 => 148
	i32 3222011088, ; 725: fr\Microsoft.TestPlatform.CrossPlatEngine.resources => 0xc00bfcd0 => 424
	i32 3226221578, ; 726: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3246114445, ; 727: tr\Microsoft.TestPlatform.CoreUtilities.resources => 0xc17bc68d => 392
	i32 3251039220, ; 728: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3255131744, ; 729: es/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xc2055e60 => 423
	i32 3258312781, ; 730: Xamarin.AndroidX.CardView => 0xc235e84d => 224
	i32 3265493905, ; 731: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 732: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 733: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 734: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 735: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3283010555, ; 736: pl\Microsoft.Testing.Extensions.MSBuild.resources => 0xc3aec3fb => 376
	i32 3286872994, ; 737: SQLite-net.dll => 0xc3e9b3a2 => 205
	i32 3290767353, ; 738: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 739: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 740: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 741: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 303
	i32 3316684772, ; 742: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 743: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 234
	i32 3317144872, ; 744: System.Data => 0xc5b79d28 => 24
	i32 3340431453, ; 745: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 222
	i32 3345895724, ; 746: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 263
	i32 3346324047, ; 747: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 260
	i32 3347930312, ; 748: Microsoft.Testing.Extensions.Telemetry.dll => 0xc78d5cc8 => 191
	i32 3348592242, ; 749: es\Microsoft.TestPlatform.CommunicationUtilities.resources => 0xc7977672 => 410
	i32 3357674450, ; 750: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 320
	i32 3358260929, ; 751: System.Text.Json => 0xc82afec1 => 137
	i32 3360279109, ; 752: SQLitePCLRaw.core => 0xc849ca45 => 207
	i32 3362336904, ; 753: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 215
	i32 3362522851, ; 754: Xamarin.AndroidX.Core => 0xc86c06e3 => 231
	i32 3366347497, ; 755: Java.Interop => 0xc8a662e9 => 168
	i32 3373364338, ; 756: zh-Hans/Microsoft.TestPlatform.CoreUtilities.resources.dll => 0xc9117472 => 393
	i32 3374999561, ; 757: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 264
	i32 3381016424, ; 758: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 299
	i32 3383023694, ; 759: ru/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0xc9a4d84e => 443
	i32 3395150330, ; 760: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 761: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3403954882, ; 762: de\Microsoft.VisualStudio.TestPlatform.Common.resources => 0xcae43ac2 => 435
	i32 3405233483, ; 763: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 235
	i32 3428513518, ; 764: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 178
	i32 3429136800, ; 765: System.Xml => 0xcc6479a0 => 163
	i32 3429687889, ; 766: zh-Hans/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0xcc6ce251 => 445
	i32 3430777524, ; 767: netstandard => 0xcc7d82b4 => 167
	i32 3440314181, ; 768: fr\Microsoft.Testing.Extensions.Telemetry.resources => 0xcd0f0745 => 333
	i32 3441283291, ; 769: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 238
	i32 3445260447, ; 770: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 771: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 185
	i32 3463511458, ; 772: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 307
	i32 3464724269, ; 773: de/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xce837f2d => 422
	i32 3471940407, ; 774: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 775: Mono.Android => 0xcf3163e6 => 171
	i32 3479583265, ; 776: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 320
	i32 3484440000, ; 777: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 319
	i32 3485117614, ; 778: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 779: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 780: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 227
	i32 3496917708, ; 781: it/Microsoft.Testing.Platform.resources.dll => 0xd06ebacc => 360
	i32 3509114376, ; 782: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 783: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 784: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 785: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3543600791, ; 786: Microsoft.TestPlatform.CoreUtilities.dll => 0xd3370e97 => 196
	i32 3553906126, ; 787: it/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0xd3d44dce => 438
	i32 3554837929, ; 788: de\Microsoft.Testing.Platform.resources => 0xd3e285a9 => 357
	i32 3560100363, ; 789: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 790: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3575270227, ; 791: zh-Hans/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0xd51a4b53 => 354
	i32 3580758918, ; 792: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 327
	i32 3581187863, ; 793: de\Microsoft.TestPlatform.CommunicationUtilities.resources => 0xd5749717 => 409
	i32 3586745494, ; 794: ja\Microsoft.Testing.Extensions.Telemetry.resources => 0xd5c96496 => 335
	i32 3597029428, ; 795: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 213
	i32 3598340787, ; 796: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3602925291, ; 797: es/Microsoft.Testing.Platform.resources.dll => 0xd6c046eb => 358
	i32 3608436895, ; 798: tr\Microsoft.VisualStudio.TestPlatform.TestFramework.resources => 0xd714609f => 457
	i32 3608519521, ; 799: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 800: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 801: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 262
	i32 3633644679, ; 802: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 217
	i32 3638274909, ; 803: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 804: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 248
	i32 3643446276, ; 805: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 324
	i32 3643854240, ; 806: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 259
	i32 3645089577, ; 807: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3648365585, ; 808: Microsoft.Testing.Platform => 0xd975a411 => 194
	i32 3657292374, ; 809: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 177
	i32 3660523487, ; 810: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3663253920, ; 811: es\Microsoft.Testing.Extensions.MSBuild.resources => 0xda58d1a0 => 371
	i32 3668376873, ; 812: zh-Hans\Microsoft.VisualStudio.TestPlatform.Common.resources => 0xdaa6fd29 => 445
	i32 3672681054, ; 813: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3678800269, ; 814: Microsoft.TestPlatform.CommunicationUtilities => 0xdb46098d => 199
	i32 3679500197, ; 815: es/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0xdb50b7a5 => 371
	i32 3682565725, ; 816: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 223
	i32 3684561358, ; 817: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 227
	i32 3691806437, ; 818: Microsoft.Testing.Extensions.Telemetry => 0xdc0c7ee5 => 191
	i32 3697841164, ; 819: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 329
	i32 3700866549, ; 820: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 821: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 232
	i32 3716563718, ; 822: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 823: Xamarin.AndroidX.Annotation => 0xdda814c6 => 216
	i32 3724971120, ; 824: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 258
	i32 3727046658, ; 825: Microsoft.TestPlatform.Utilities.dll => 0xde263802 => 201
	i32 3732100267, ; 826: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 827: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 828: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3750392659, ; 829: ru/Microsoft.Testing.Extensions.MSBuild.resources.dll => 0xdf8a7353 => 378
	i32 3751444290, ; 830: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3754567612, ; 831: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 209
	i32 3759409350, ; 832: cs\Microsoft.Testing.Platform.resources => 0xe01408c6 => 356
	i32 3783630935, ; 833: ja\Microsoft.Testing.Extensions.MSBuild.resources => 0xe185a057 => 374
	i32 3786282454, ; 834: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 225
	i32 3792276235, ; 835: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 836: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 185
	i32 3802395368, ; 837: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3810897912, ; 838: fr\Microsoft.TestPlatform.CommunicationUtilities.resources => 0xe325aff8 => 411
	i32 3816825644, ; 839: cs/Microsoft.Testing.Extensions.Telemetry.resources.dll => 0xe380232c => 330
	i32 3817053629, ; 840: tr/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0xe3839dbd => 405
	i32 3819260425, ; 841: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 842: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3825535149, ; 843: Microsoft.Testing.Extensions.VSTestBridge.dll => 0xe40508ad => 193
	i32 3829621856, ; 844: System.Numerics.dll => 0xe4436460 => 83
	i32 3831657003, ; 845: fr\Microsoft.Testing.Platform.resources => 0xe462722b => 359
	i32 3837488932, ; 846: de\Microsoft.TestPlatform.CoreUtilities.resources => 0xe4bb6f24 => 383
	i32 3841636137, ; 847: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 179
	i32 3844307129, ; 848: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 849: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3864064308, ; 850: ko\Microsoft.VisualStudio.TestPlatform.Common.resources => 0xe650f134 => 440
	i32 3870376305, ; 851: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 852: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 853: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3876362041, ; 854: SQLite-net => 0xe70c9739 => 205
	i32 3879343429, ; 855: zh-Hans\Microsoft.Testing.Platform.resources => 0xe73a1545 => 367
	i32 3885497537, ; 856: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 857: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 273
	i32 3888767677, ; 858: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 263
	i32 3889960447, ; 859: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 328
	i32 3893759750, ; 860: zh-Hans\Microsoft.TestPlatform.CrossPlatEngine.resources => 0xe8160f06 => 432
	i32 3896106733, ; 861: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 862: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 231
	i32 3898101830, ; 863: ru\Microsoft.TestPlatform.CommunicationUtilities.resources => 0xe8585046 => 417
	i32 3901907137, ; 864: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 865: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 866: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 276
	i32 3928044579, ; 867: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 868: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 869: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 261
	i32 3945713374, ; 870: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 871: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 872: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 219
	i32 3959773229, ; 873: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 250
	i32 3977513776, ; 874: Microsoft.VisualStudio.TestPlatform.TestFramework => 0xed140b30 => 203
	i32 3980434154, ; 875: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 323
	i32 3987592930, ; 876: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 305
	i32 4003436829, ; 877: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4015948917, ; 878: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 218
	i32 4025784931, ; 879: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 880: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 187
	i32 4054681211, ; 881: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 882: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 883: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4084497668, ; 884: cs/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0xf3747d04 => 395
	i32 4093880443, ; 885: ru/Microsoft.VisualStudio.TestPlatform.TestFramework.resources.dll => 0xf403a87b => 456
	i32 4094352644, ; 886: Microsoft.Maui.Essentials.dll => 0xf40add04 => 189
	i32 4099507663, ; 887: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 888: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 889: Xamarin.AndroidX.Emoji2 => 0xf479582c => 239
	i32 4102112229, ; 890: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 318
	i32 4107499832, ; 891: tr/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0xf4d37938 => 353
	i32 4125707920, ; 892: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 313
	i32 4126470640, ; 893: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 178
	i32 4127667938, ; 894: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130298413, ; 895: ko/Microsoft.VisualStudio.TestPlatform.Common.resources.dll => 0xf62f5a2d => 440
	i32 4130442656, ; 896: System.AppContext => 0xf6318da0 => 6
	i32 4132566263, ; 897: ja\Microsoft.TestPlatform.CoreUtilities.resources => 0xf651f4f7 => 387
	i32 4139544265, ; 898: tr/Microsoft.TestPlatform.CommunicationUtilities.resources.dll => 0xf6bc6ec9 => 418
	i32 4147896353, ; 899: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 900: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 325
	i32 4151237749, ; 901: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 902: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 903: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 904: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4165445317, ; 905: Microsoft.Testing.Extensions.TrxReport.Abstractions.dll => 0xf847a6c5 => 192
	i32 4181436372, ; 906: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 907: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 255
	i32 4185676441, ; 908: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 909: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 910: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4227851409, ; 911: ja/Microsoft.VisualStudio.TestPlatform.ObjectModel.resources.dll => 0xfbffe491 => 400
	i32 4231935821, ; 912: pl\Microsoft.VisualStudio.TestPlatform.ObjectModel.resources => 0xfc3e374d => 402
	i32 4235817225, ; 913: fr\Microsoft.VisualStudio.TestPlatform.Common.resources => 0xfc797109 => 437
	i32 4236519394, ; 914: Microsoft.TestPlatform.CrossPlatEngine => 0xfc8427e2 => 200
	i32 4244015775, ; 915: es\Microsoft.VisualStudio.TestPlatform.Common.resources => 0xfcf68a9f => 436
	i32 4246171057, ; 916: pl/Microsoft.Testing.Extensions.VSTestBridge.resources.dll => 0xfd176db1 => 350
	i32 4256097574, ; 917: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 232
	i32 4258378803, ; 918: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 254
	i32 4260525087, ; 919: System.Buffers => 0xfdf2741f => 7
	i32 4270170710, ; 920: cs/Microsoft.TestPlatform.CrossPlatEngine.resources.dll => 0xfe85a256 => 421
	i32 4271975918, ; 921: Microsoft.Maui.Controls.dll => 0xfea12dee => 186
	i32 4274670180, ; 922: es\Microsoft.Testing.Platform.resources => 0xfeca4a64 => 358
	i32 4274976490, ; 923: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 924: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 255
	i32 4294763496 ; 925: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 241
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [926 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 331, ; 2
	i32 108, ; 3
	i32 251, ; 4
	i32 285, ; 5
	i32 48, ; 6
	i32 204, ; 7
	i32 80, ; 8
	i32 145, ; 9
	i32 460, ; 10
	i32 30, ; 11
	i32 329, ; 12
	i32 124, ; 13
	i32 190, ; 14
	i32 102, ; 15
	i32 269, ; 16
	i32 406, ; 17
	i32 107, ; 18
	i32 269, ; 19
	i32 139, ; 20
	i32 195, ; 21
	i32 289, ; 22
	i32 77, ; 23
	i32 124, ; 24
	i32 13, ; 25
	i32 225, ; 26
	i32 399, ; 27
	i32 373, ; 28
	i32 132, ; 29
	i32 336, ; 30
	i32 271, ; 31
	i32 151, ; 32
	i32 403, ; 33
	i32 326, ; 34
	i32 327, ; 35
	i32 390, ; 36
	i32 18, ; 37
	i32 430, ; 38
	i32 223, ; 39
	i32 434, ; 40
	i32 26, ; 41
	i32 245, ; 42
	i32 1, ; 43
	i32 59, ; 44
	i32 42, ; 45
	i32 91, ; 46
	i32 228, ; 47
	i32 147, ; 48
	i32 349, ; 49
	i32 247, ; 50
	i32 244, ; 51
	i32 298, ; 52
	i32 413, ; 53
	i32 54, ; 54
	i32 69, ; 55
	i32 399, ; 56
	i32 395, ; 57
	i32 384, ; 58
	i32 326, ; 59
	i32 214, ; 60
	i32 430, ; 61
	i32 83, ; 62
	i32 453, ; 63
	i32 311, ; 64
	i32 342, ; 65
	i32 246, ; 66
	i32 208, ; 67
	i32 377, ; 68
	i32 310, ; 69
	i32 131, ; 70
	i32 55, ; 71
	i32 149, ; 72
	i32 74, ; 73
	i32 145, ; 74
	i32 62, ; 75
	i32 431, ; 76
	i32 146, ; 77
	i32 461, ; 78
	i32 413, ; 79
	i32 165, ; 80
	i32 407, ; 81
	i32 362, ; 82
	i32 449, ; 83
	i32 343, ; 84
	i32 322, ; 85
	i32 426, ; 86
	i32 229, ; 87
	i32 12, ; 88
	i32 435, ; 89
	i32 242, ; 90
	i32 125, ; 91
	i32 152, ; 92
	i32 389, ; 93
	i32 113, ; 94
	i32 166, ; 95
	i32 164, ; 96
	i32 244, ; 97
	i32 257, ; 98
	i32 84, ; 99
	i32 309, ; 100
	i32 303, ; 101
	i32 202, ; 102
	i32 184, ; 103
	i32 347, ; 104
	i32 387, ; 105
	i32 150, ; 106
	i32 289, ; 107
	i32 60, ; 108
	i32 180, ; 109
	i32 51, ; 110
	i32 103, ; 111
	i32 114, ; 112
	i32 460, ; 113
	i32 40, ; 114
	i32 452, ; 115
	i32 282, ; 116
	i32 280, ; 117
	i32 202, ; 118
	i32 120, ; 119
	i32 433, ; 120
	i32 394, ; 121
	i32 317, ; 122
	i32 201, ; 123
	i32 52, ; 124
	i32 388, ; 125
	i32 44, ; 126
	i32 119, ; 127
	i32 458, ; 128
	i32 234, ; 129
	i32 315, ; 130
	i32 240, ; 131
	i32 174, ; 132
	i32 81, ; 133
	i32 197, ; 134
	i32 136, ; 135
	i32 193, ; 136
	i32 276, ; 137
	i32 221, ; 138
	i32 439, ; 139
	i32 8, ; 140
	i32 73, ; 141
	i32 297, ; 142
	i32 155, ; 143
	i32 291, ; 144
	i32 154, ; 145
	i32 92, ; 146
	i32 286, ; 147
	i32 45, ; 148
	i32 312, ; 149
	i32 338, ; 150
	i32 300, ; 151
	i32 290, ; 152
	i32 109, ; 153
	i32 129, ; 154
	i32 206, ; 155
	i32 294, ; 156
	i32 360, ; 157
	i32 25, ; 158
	i32 211, ; 159
	i32 451, ; 160
	i32 72, ; 161
	i32 55, ; 162
	i32 46, ; 163
	i32 321, ; 164
	i32 427, ; 165
	i32 183, ; 166
	i32 235, ; 167
	i32 199, ; 168
	i32 175, ; 169
	i32 346, ; 170
	i32 402, ; 171
	i32 22, ; 172
	i32 249, ; 173
	i32 337, ; 174
	i32 382, ; 175
	i32 458, ; 176
	i32 86, ; 177
	i32 43, ; 178
	i32 160, ; 179
	i32 71, ; 180
	i32 372, ; 181
	i32 262, ; 182
	i32 364, ; 183
	i32 436, ; 184
	i32 198, ; 185
	i32 3, ; 186
	i32 42, ; 187
	i32 334, ; 188
	i32 63, ; 189
	i32 442, ; 190
	i32 428, ; 191
	i32 16, ; 192
	i32 437, ; 193
	i32 420, ; 194
	i32 53, ; 195
	i32 324, ; 196
	i32 285, ; 197
	i32 369, ; 198
	i32 355, ; 199
	i32 105, ; 200
	i32 204, ; 201
	i32 290, ; 202
	i32 366, ; 203
	i32 283, ; 204
	i32 246, ; 205
	i32 362, ; 206
	i32 34, ; 207
	i32 158, ; 208
	i32 459, ; 209
	i32 85, ; 210
	i32 32, ; 211
	i32 12, ; 212
	i32 51, ; 213
	i32 351, ; 214
	i32 56, ; 215
	i32 266, ; 216
	i32 441, ; 217
	i32 36, ; 218
	i32 179, ; 219
	i32 299, ; 220
	i32 284, ; 221
	i32 411, ; 222
	i32 219, ; 223
	i32 35, ; 224
	i32 58, ; 225
	i32 253, ; 226
	i32 370, ; 227
	i32 340, ; 228
	i32 173, ; 229
	i32 333, ; 230
	i32 17, ; 231
	i32 394, ; 232
	i32 287, ; 233
	i32 398, ; 234
	i32 164, ; 235
	i32 401, ; 236
	i32 450, ; 237
	i32 295, ; 238
	i32 375, ; 239
	i32 459, ; 240
	i32 353, ; 241
	i32 312, ; 242
	i32 252, ; 243
	i32 182, ; 244
	i32 279, ; 245
	i32 422, ; 246
	i32 373, ; 247
	i32 418, ; 248
	i32 318, ; 249
	i32 153, ; 250
	i32 408, ; 251
	i32 275, ; 252
	i32 260, ; 253
	i32 382, ; 254
	i32 405, ; 255
	i32 316, ; 256
	i32 221, ; 257
	i32 29, ; 258
	i32 293, ; 259
	i32 352, ; 260
	i32 52, ; 261
	i32 194, ; 262
	i32 456, ; 263
	i32 407, ; 264
	i32 314, ; 265
	i32 377, ; 266
	i32 425, ; 267
	i32 280, ; 268
	i32 5, ; 269
	i32 298, ; 270
	i32 341, ; 271
	i32 406, ; 272
	i32 270, ; 273
	i32 274, ; 274
	i32 226, ; 275
	i32 291, ; 276
	i32 218, ; 277
	i32 378, ; 278
	i32 414, ; 279
	i32 295, ; 280
	i32 197, ; 281
	i32 335, ; 282
	i32 444, ; 283
	i32 207, ; 284
	i32 237, ; 285
	i32 349, ; 286
	i32 85, ; 287
	i32 447, ; 288
	i32 350, ; 289
	i32 338, ; 290
	i32 279, ; 291
	i32 61, ; 292
	i32 438, ; 293
	i32 112, ; 294
	i32 195, ; 295
	i32 57, ; 296
	i32 441, ; 297
	i32 328, ; 298
	i32 266, ; 299
	i32 99, ; 300
	i32 19, ; 301
	i32 230, ; 302
	i32 344, ; 303
	i32 111, ; 304
	i32 348, ; 305
	i32 101, ; 306
	i32 454, ; 307
	i32 102, ; 308
	i32 348, ; 309
	i32 455, ; 310
	i32 296, ; 311
	i32 104, ; 312
	i32 455, ; 313
	i32 283, ; 314
	i32 71, ; 315
	i32 340, ; 316
	i32 38, ; 317
	i32 32, ; 318
	i32 103, ; 319
	i32 73, ; 320
	i32 302, ; 321
	i32 9, ; 322
	i32 123, ; 323
	i32 403, ; 324
	i32 46, ; 325
	i32 446, ; 326
	i32 220, ; 327
	i32 184, ; 328
	i32 9, ; 329
	i32 43, ; 330
	i32 4, ; 331
	i32 267, ; 332
	i32 306, ; 333
	i32 0, ; 334
	i32 331, ; 335
	i32 301, ; 336
	i32 31, ; 337
	i32 339, ; 338
	i32 138, ; 339
	i32 92, ; 340
	i32 93, ; 341
	i32 321, ; 342
	i32 433, ; 343
	i32 415, ; 344
	i32 49, ; 345
	i32 141, ; 346
	i32 112, ; 347
	i32 140, ; 348
	i32 236, ; 349
	i32 410, ; 350
	i32 200, ; 351
	i32 115, ; 352
	i32 409, ; 353
	i32 284, ; 354
	i32 157, ; 355
	i32 76, ; 356
	i32 351, ; 357
	i32 79, ; 358
	i32 256, ; 359
	i32 37, ; 360
	i32 278, ; 361
	i32 240, ; 362
	i32 233, ; 363
	i32 64, ; 364
	i32 138, ; 365
	i32 15, ; 366
	i32 116, ; 367
	i32 272, ; 368
	i32 281, ; 369
	i32 228, ; 370
	i32 400, ; 371
	i32 48, ; 372
	i32 70, ; 373
	i32 80, ; 374
	i32 126, ; 375
	i32 94, ; 376
	i32 396, ; 377
	i32 451, ; 378
	i32 121, ; 379
	i32 288, ; 380
	i32 26, ; 381
	i32 391, ; 382
	i32 208, ; 383
	i32 249, ; 384
	i32 97, ; 385
	i32 28, ; 386
	i32 224, ; 387
	i32 319, ; 388
	i32 297, ; 389
	i32 149, ; 390
	i32 169, ; 391
	i32 356, ; 392
	i32 4, ; 393
	i32 98, ; 394
	i32 33, ; 395
	i32 374, ; 396
	i32 93, ; 397
	i32 271, ; 398
	i32 180, ; 399
	i32 423, ; 400
	i32 21, ; 401
	i32 41, ; 402
	i32 170, ; 403
	i32 313, ; 404
	i32 242, ; 405
	i32 428, ; 406
	i32 305, ; 407
	i32 354, ; 408
	i32 370, ; 409
	i32 256, ; 410
	i32 385, ; 411
	i32 287, ; 412
	i32 281, ; 413
	i32 261, ; 414
	i32 2, ; 415
	i32 134, ; 416
	i32 111, ; 417
	i32 345, ; 418
	i32 181, ; 419
	i32 325, ; 420
	i32 211, ; 421
	i32 174, ; 422
	i32 322, ; 423
	i32 58, ; 424
	i32 95, ; 425
	i32 304, ; 426
	i32 363, ; 427
	i32 39, ; 428
	i32 389, ; 429
	i32 293, ; 430
	i32 222, ; 431
	i32 25, ; 432
	i32 94, ; 433
	i32 89, ; 434
	i32 99, ; 435
	i32 10, ; 436
	i32 386, ; 437
	i32 347, ; 438
	i32 448, ; 439
	i32 87, ; 440
	i32 203, ; 441
	i32 100, ; 442
	i32 380, ; 443
	i32 268, ; 444
	i32 176, ; 445
	i32 288, ; 446
	i32 213, ; 447
	i32 444, ; 448
	i32 301, ; 449
	i32 7, ; 450
	i32 343, ; 451
	i32 253, ; 452
	i32 415, ; 453
	i32 296, ; 454
	i32 210, ; 455
	i32 375, ; 456
	i32 88, ; 457
	i32 396, ; 458
	i32 248, ; 459
	i32 414, ; 460
	i32 154, ; 461
	i32 300, ; 462
	i32 33, ; 463
	i32 116, ; 464
	i32 82, ; 465
	i32 209, ; 466
	i32 20, ; 467
	i32 416, ; 468
	i32 11, ; 469
	i32 162, ; 470
	i32 3, ; 471
	i32 0, ; 472
	i32 188, ; 473
	i32 429, ; 474
	i32 308, ; 475
	i32 364, ; 476
	i32 183, ; 477
	i32 181, ; 478
	i32 84, ; 479
	i32 292, ; 480
	i32 64, ; 481
	i32 345, ; 482
	i32 310, ; 483
	i32 275, ; 484
	i32 143, ; 485
	i32 363, ; 486
	i32 257, ; 487
	i32 157, ; 488
	i32 427, ; 489
	i32 41, ; 490
	i32 117, ; 491
	i32 449, ; 492
	i32 177, ; 493
	i32 212, ; 494
	i32 359, ; 495
	i32 304, ; 496
	i32 264, ; 497
	i32 131, ; 498
	i32 75, ; 499
	i32 381, ; 500
	i32 66, ; 501
	i32 314, ; 502
	i32 457, ; 503
	i32 172, ; 504
	i32 196, ; 505
	i32 454, ; 506
	i32 216, ; 507
	i32 143, ; 508
	i32 341, ; 509
	i32 106, ; 510
	i32 151, ; 511
	i32 330, ; 512
	i32 70, ; 513
	i32 448, ; 514
	i32 447, ; 515
	i32 404, ; 516
	i32 421, ; 517
	i32 156, ; 518
	i32 176, ; 519
	i32 121, ; 520
	i32 127, ; 521
	i32 309, ; 522
	i32 152, ; 523
	i32 239, ; 524
	i32 141, ; 525
	i32 226, ; 526
	i32 404, ; 527
	i32 306, ; 528
	i32 20, ; 529
	i32 14, ; 530
	i32 412, ; 531
	i32 368, ; 532
	i32 135, ; 533
	i32 75, ; 534
	i32 59, ; 535
	i32 206, ; 536
	i32 229, ; 537
	i32 167, ; 538
	i32 168, ; 539
	i32 419, ; 540
	i32 294, ; 541
	i32 186, ; 542
	i32 192, ; 543
	i32 443, ; 544
	i32 15, ; 545
	i32 74, ; 546
	i32 6, ; 547
	i32 334, ; 548
	i32 442, ; 549
	i32 23, ; 550
	i32 251, ; 551
	i32 210, ; 552
	i32 344, ; 553
	i32 91, ; 554
	i32 365, ; 555
	i32 307, ; 556
	i32 1, ; 557
	i32 385, ; 558
	i32 136, ; 559
	i32 252, ; 560
	i32 274, ; 561
	i32 134, ; 562
	i32 69, ; 563
	i32 429, ; 564
	i32 146, ; 565
	i32 361, ; 566
	i32 316, ; 567
	i32 292, ; 568
	i32 462, ; 569
	i32 198, ; 570
	i32 243, ; 571
	i32 182, ; 572
	i32 88, ; 573
	i32 96, ; 574
	i32 355, ; 575
	i32 233, ; 576
	i32 332, ; 577
	i32 238, ; 578
	i32 311, ; 579
	i32 31, ; 580
	i32 45, ; 581
	i32 247, ; 582
	i32 408, ; 583
	i32 372, ; 584
	i32 439, ; 585
	i32 339, ; 586
	i32 383, ; 587
	i32 446, ; 588
	i32 357, ; 589
	i32 212, ; 590
	i32 109, ; 591
	i32 416, ; 592
	i32 158, ; 593
	i32 35, ; 594
	i32 22, ; 595
	i32 453, ; 596
	i32 114, ; 597
	i32 431, ; 598
	i32 57, ; 599
	i32 272, ; 600
	i32 450, ; 601
	i32 144, ; 602
	i32 118, ; 603
	i32 397, ; 604
	i32 120, ; 605
	i32 110, ; 606
	i32 214, ; 607
	i32 139, ; 608
	i32 220, ; 609
	i32 54, ; 610
	i32 105, ; 611
	i32 317, ; 612
	i32 187, ; 613
	i32 188, ; 614
	i32 133, ; 615
	i32 286, ; 616
	i32 277, ; 617
	i32 265, ; 618
	i32 323, ; 619
	i32 243, ; 620
	i32 190, ; 621
	i32 159, ; 622
	i32 302, ; 623
	i32 412, ; 624
	i32 230, ; 625
	i32 424, ; 626
	i32 376, ; 627
	i32 462, ; 628
	i32 163, ; 629
	i32 132, ; 630
	i32 265, ; 631
	i32 161, ; 632
	i32 175, ; 633
	i32 315, ; 634
	i32 426, ; 635
	i32 254, ; 636
	i32 140, ; 637
	i32 393, ; 638
	i32 277, ; 639
	i32 273, ; 640
	i32 419, ; 641
	i32 391, ; 642
	i32 169, ; 643
	i32 189, ; 644
	i32 452, ; 645
	i32 215, ; 646
	i32 282, ; 647
	i32 40, ; 648
	i32 380, ; 649
	i32 241, ; 650
	i32 81, ; 651
	i32 368, ; 652
	i32 379, ; 653
	i32 56, ; 654
	i32 369, ; 655
	i32 37, ; 656
	i32 97, ; 657
	i32 166, ; 658
	i32 172, ; 659
	i32 420, ; 660
	i32 278, ; 661
	i32 337, ; 662
	i32 82, ; 663
	i32 367, ; 664
	i32 217, ; 665
	i32 390, ; 666
	i32 98, ; 667
	i32 401, ; 668
	i32 30, ; 669
	i32 159, ; 670
	i32 381, ; 671
	i32 417, ; 672
	i32 18, ; 673
	i32 432, ; 674
	i32 127, ; 675
	i32 366, ; 676
	i32 119, ; 677
	i32 237, ; 678
	i32 268, ; 679
	i32 250, ; 680
	i32 386, ; 681
	i32 270, ; 682
	i32 165, ; 683
	i32 245, ; 684
	i32 397, ; 685
	i32 461, ; 686
	i32 336, ; 687
	i32 398, ; 688
	i32 267, ; 689
	i32 258, ; 690
	i32 170, ; 691
	i32 16, ; 692
	i32 384, ; 693
	i32 144, ; 694
	i32 392, ; 695
	i32 308, ; 696
	i32 342, ; 697
	i32 361, ; 698
	i32 125, ; 699
	i32 118, ; 700
	i32 38, ; 701
	i32 115, ; 702
	i32 47, ; 703
	i32 142, ; 704
	i32 352, ; 705
	i32 117, ; 706
	i32 34, ; 707
	i32 173, ; 708
	i32 379, ; 709
	i32 434, ; 710
	i32 332, ; 711
	i32 95, ; 712
	i32 53, ; 713
	i32 346, ; 714
	i32 259, ; 715
	i32 365, ; 716
	i32 388, ; 717
	i32 129, ; 718
	i32 153, ; 719
	i32 24, ; 720
	i32 161, ; 721
	i32 236, ; 722
	i32 425, ; 723
	i32 148, ; 724
	i32 424, ; 725
	i32 104, ; 726
	i32 392, ; 727
	i32 89, ; 728
	i32 423, ; 729
	i32 224, ; 730
	i32 60, ; 731
	i32 142, ; 732
	i32 100, ; 733
	i32 5, ; 734
	i32 13, ; 735
	i32 376, ; 736
	i32 205, ; 737
	i32 122, ; 738
	i32 135, ; 739
	i32 28, ; 740
	i32 303, ; 741
	i32 72, ; 742
	i32 234, ; 743
	i32 24, ; 744
	i32 222, ; 745
	i32 263, ; 746
	i32 260, ; 747
	i32 191, ; 748
	i32 410, ; 749
	i32 320, ; 750
	i32 137, ; 751
	i32 207, ; 752
	i32 215, ; 753
	i32 231, ; 754
	i32 168, ; 755
	i32 393, ; 756
	i32 264, ; 757
	i32 299, ; 758
	i32 443, ; 759
	i32 101, ; 760
	i32 123, ; 761
	i32 435, ; 762
	i32 235, ; 763
	i32 178, ; 764
	i32 163, ; 765
	i32 445, ; 766
	i32 167, ; 767
	i32 333, ; 768
	i32 238, ; 769
	i32 39, ; 770
	i32 185, ; 771
	i32 307, ; 772
	i32 422, ; 773
	i32 17, ; 774
	i32 171, ; 775
	i32 320, ; 776
	i32 319, ; 777
	i32 137, ; 778
	i32 150, ; 779
	i32 227, ; 780
	i32 360, ; 781
	i32 155, ; 782
	i32 130, ; 783
	i32 19, ; 784
	i32 65, ; 785
	i32 196, ; 786
	i32 438, ; 787
	i32 357, ; 788
	i32 147, ; 789
	i32 47, ; 790
	i32 354, ; 791
	i32 327, ; 792
	i32 409, ; 793
	i32 335, ; 794
	i32 213, ; 795
	i32 79, ; 796
	i32 358, ; 797
	i32 457, ; 798
	i32 61, ; 799
	i32 106, ; 800
	i32 262, ; 801
	i32 217, ; 802
	i32 49, ; 803
	i32 248, ; 804
	i32 324, ; 805
	i32 259, ; 806
	i32 14, ; 807
	i32 194, ; 808
	i32 177, ; 809
	i32 68, ; 810
	i32 371, ; 811
	i32 445, ; 812
	i32 171, ; 813
	i32 199, ; 814
	i32 371, ; 815
	i32 223, ; 816
	i32 227, ; 817
	i32 191, ; 818
	i32 329, ; 819
	i32 78, ; 820
	i32 232, ; 821
	i32 108, ; 822
	i32 216, ; 823
	i32 258, ; 824
	i32 201, ; 825
	i32 67, ; 826
	i32 63, ; 827
	i32 27, ; 828
	i32 378, ; 829
	i32 160, ; 830
	i32 209, ; 831
	i32 356, ; 832
	i32 374, ; 833
	i32 225, ; 834
	i32 10, ; 835
	i32 185, ; 836
	i32 11, ; 837
	i32 411, ; 838
	i32 330, ; 839
	i32 405, ; 840
	i32 78, ; 841
	i32 126, ; 842
	i32 193, ; 843
	i32 83, ; 844
	i32 359, ; 845
	i32 383, ; 846
	i32 179, ; 847
	i32 66, ; 848
	i32 107, ; 849
	i32 440, ; 850
	i32 65, ; 851
	i32 128, ; 852
	i32 122, ; 853
	i32 205, ; 854
	i32 367, ; 855
	i32 77, ; 856
	i32 273, ; 857
	i32 263, ; 858
	i32 328, ; 859
	i32 432, ; 860
	i32 8, ; 861
	i32 231, ; 862
	i32 417, ; 863
	i32 2, ; 864
	i32 44, ; 865
	i32 276, ; 866
	i32 156, ; 867
	i32 128, ; 868
	i32 261, ; 869
	i32 23, ; 870
	i32 133, ; 871
	i32 219, ; 872
	i32 250, ; 873
	i32 203, ; 874
	i32 323, ; 875
	i32 305, ; 876
	i32 29, ; 877
	i32 218, ; 878
	i32 62, ; 879
	i32 187, ; 880
	i32 90, ; 881
	i32 87, ; 882
	i32 148, ; 883
	i32 395, ; 884
	i32 456, ; 885
	i32 189, ; 886
	i32 36, ; 887
	i32 86, ; 888
	i32 239, ; 889
	i32 318, ; 890
	i32 353, ; 891
	i32 313, ; 892
	i32 178, ; 893
	i32 50, ; 894
	i32 440, ; 895
	i32 6, ; 896
	i32 387, ; 897
	i32 418, ; 898
	i32 90, ; 899
	i32 325, ; 900
	i32 21, ; 901
	i32 162, ; 902
	i32 96, ; 903
	i32 50, ; 904
	i32 192, ; 905
	i32 113, ; 906
	i32 255, ; 907
	i32 130, ; 908
	i32 76, ; 909
	i32 27, ; 910
	i32 400, ; 911
	i32 402, ; 912
	i32 437, ; 913
	i32 200, ; 914
	i32 436, ; 915
	i32 350, ; 916
	i32 232, ; 917
	i32 254, ; 918
	i32 7, ; 919
	i32 421, ; 920
	i32 186, ; 921
	i32 358, ; 922
	i32 110, ; 923
	i32 255, ; 924
	i32 241 ; 925
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nofree norecurse nosync nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: "no-trapping-math"="true" noreturn nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nofree norecurse nosync nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { "no-trapping-math"="true" noreturn nounwind "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ a8cd27e430e55df3e3c1e3a43d35c11d9512a2db"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"NumRegisterParameters", i32 0}
