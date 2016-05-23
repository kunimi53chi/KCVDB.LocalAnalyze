# KCVDB.LocalAnalyze
艦これ検証DBのログデータをローカルで数値解析するためのライブラリ。MITライセンス  
**64ビット環境**でのみ動作します。これはログファイルを並列的に計算するとメモリー2GBの制約により、64ビット環境がほぼ必須になるためです。

##ファイルの読み込み
**KCVDBLogFile**クラス：System.IO.Fileと同じような感覚で使います。

- ReadAllText : .logファイルまたはgzipで圧縮された.logファイル（.gzファイル）の全てのテキストを読み込みます
- ReadAllLines : 　　　　〃　　行単位でテキストを読み込みます
- ParseAllLines : 　　　　〃　　行単位でTSVを項目ごとにSplitしてパースしながら読みます

この他にもgzipの解凍ラッパーメソッドがあります（いちいちusingするの面倒だったから）

**LogDirectory**クラス：7z対応に対応したファイル読み込み  
7x.dllおよびRx-Mainに依存しています。利用する場合はアプリケーションプロジェクトに以下の操作を加えてください。

- `プロパティ`→`ビルド イベント`→`ビルド後イベントのコマンド ライン`を以下のように編集  
```
XCOPY /S /I /Q /Y "<KCVDB.LocalAnalyzeプロジェクトディレクトリへのパス>x86" "$(TargetDir)x86"
XCOPY /S /I /Q /Y "<KCVDB.LocalAnalyzeプロジェクトディレクトリへのパス>x64" "$(TargetDir)x64"
```
- `NuGet パッケージの管理`→`参照`→検索に`Rx-Main`と入力→`Rx-Main`を選択→`インストール`

使用法についてはKCVDB.LocalAnalyze.Testを参考にしてください。

##統計用計算
**Statics**クラス：現状ベルヌーイ試行の95％信頼区間を計算できるだけ_(:3ゝ∠)_

##日付処理
**DateTimerHelper**クラス : RFC1123形式の日付文字列のパースや、日本標準時（JST）への変換を実装しています
