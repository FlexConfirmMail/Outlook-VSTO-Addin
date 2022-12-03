FlexConfirmMail for Outlook
===========================

メールの誤送信を防止するためのOutlook向けアドオンです。



![FlexConfirmMail logo](doc/Guide/fcm-logo.png)

**主な機能**

 * メールの送信時に、宛先を確認するダイアログを表示します。
 * Office 2016/2019/2021 + Microsoft 365をサポートしています。

**使い方**

* 利用方法は公式サイトで詳しく解説しています。  
  https://www.flexconfirmmail.com

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
