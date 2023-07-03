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
    * `Workloads > Office/Sharepoint Development`（`ワークロード > Office/SharePoint 開発`
    * `個別のコンポーネント > Visual Studio Tools for Office (VSTO)`
 3. 開発用のOfficeアプリを用意します（任意）
    * コンパイルのみであれば必要ありませんが、用意すると開発上は便利です。
 4. Visual StudioでFlexConfirmMailプロジェクトを開きます。
 5. VSTO開発用のダミー証明書を生成します
    * ソリューションエクスプローラでFlexConfirmMailを右クリックします。
    * Properties > Signing （プロパティ > 署名）を順に選択します。
    * 証明書欄に何か妥当な証明書の情報が表示されている場合、ダミー証明書はすでに生成済みです。
      証明書の情報が表示されていない場合、ダミー証明書を作成します。
        1. Create Test Certificate（テスト証明書の作成）をクリックします。
        2. テスト証明書の作成画面が開かれるので、何も入力せず「OK」ボタンを押して操作を確定します（（パスワードは不要です）。
           * `アクセスが拒否されました。（HRESULTからの例外: 0.x80070005(E_ACCESSDENIED)）` のようなメッセージが表示されて証明書の作成に失敗する場合、以下の手順で代替可能です。
             1. PowerShellを起動し、`New-SelfSignedCertificate -Subject "CN=FlexConfirmMailSelfSign" -Type CodeSigningCert -CertStoreLocation "Cert:\CurrentUser\My" -NotAfter (Get-Date).AddYears(10)` を実行します。
             2. Visual Studioのウィンドウに戻り、Choose from Store（ストアから選択）をクリックします。
             3. 証明書データベースのクライアント証明書のうち1つが表示されます。
               「FlexConfirmMailSelfSign」が表示されている場合、「OK」ボタンをクリックして選択します。
               そうでない場合、「その他」をクリックしてクライアント証明書の一覧から「FlexConfirmMailSelfSign」を選択し、「OK」ボタンをクリックして選択します。
 6. ビルドを開始します。

**リリース**

1. 以下のファイルに埋め込まれているバージョン情報を更新する。
   * Global.cs
   * Global.public.cs
   * FlexConfirmMail.iss
2. Visual Studio 2019でビルドする。
3. `FlexConfirmMail.iss`をInno Setupでビルドする。
4. `dist`フォルダー配下に作成されたインストーラを公開する。
