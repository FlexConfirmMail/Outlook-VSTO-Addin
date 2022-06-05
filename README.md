FlexConfirmMail for Outlook
===========================

メールの誤送信を防止するためのOutlook向けアドオンです。

![FlexConfirmMail logo](doc/Guide/fcm-logo.png)

 * 公式サイト: https://flexconfirmmail.github.io/
 * インストーラ: https://github.com/FlexConfirmMail/Outlook/releases

**主な機能**

 * メールの送信時に、宛先を確認するダイアログを表示します。
 * チェックリスト形式で確認することで送信ミスを防ぎます。
 * Office 2013/2016/2019をサポートしています。

**使い方**

 1. リリースページからFlexConfirmMailの最新版のインストーラを取得します。
 2. 導入対象の端末でインストーラを実行します。
 3. Outlookを立ち上げて動作を確認します。

**開発手順**

 1. 開発用のVisual Studio 2019環境を用意します。
    * Visual Studio 2019をターゲットにしています。
 2. Visual Studio Installerで次のコンポーネントを追加します。
    * `Workloads > Office/Sharepoint Development`
    * `個別のコンポーネント > Visual Studio Tools for Office (VSTO)`
 3. 開発用のOfficeアプリを用意します（任意）
    * コンパイルのみであれば必要ありませんが、用意すると開発上は便利です。
 4. Visual StudioでFlexConfirmMailプロジェクトを開きます。
 5. VSTO開発用のダミー証明書を生成します
    * ソリューションエクスプローラでFlexConfirmMailを右クリックします。
    * Properties > Signing > Create Test Certificate を順に選択します。
    * OKを押して確定します（パスワードは不要です）
 6. ビルドを開始します。
