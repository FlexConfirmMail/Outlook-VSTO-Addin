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
    * [Microsoftの開発者プログラム](https://developer.microsoft.com/en-us/microsoft-365/dev-program)があり、サインアップすると開発用のAzure AD組織を作ることができ、かつ無償でMS365版のOfficeを使えます。組織は90日使わないと無効化されます。
 4. Inno Setupの最新版をインストールします（インストーラをビルドする場合）
    * `Path`環境変数にInno Setupのパス（標準では`C:\Program Files (x86)\Inno Setup 6`）を追加します。
    * Inno Setupのバージョンにより生成されるインストーラのサイズが変わってきますので注意して下さい。本記事執筆時点の最新版は6.2.2です。
 5. Visual StudioでFlexConfirmMailプロジェクトを開きます。
 6. VSTO開発用のダミー証明書を生成します
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
 7. ビルドを開始します。

**リリース**

1. 以下のファイルに埋め込まれているバージョン情報を更新します。
   * Global.cs
   * FlexConfirmMail.iss
2. 上記をコミット・プッシュします。
3. アドオン署名用の証明書を公開用のものに切り替えます。
    * Properties > Signing （プロパティ > 署名）
    * デフォルトで設定されている証明書は製品用のものです。公開用証明書に切り替えたFlexConfirmMail.csprojはコミット・プッシュしないで下さい。
4. make.batを実行します。
    * 公開版のインストーラは`public`フォルダー配下に作成されます。
    * 製品版をビルドする場合はmake-signed.batを使用します。
        * 製品版用のコードサイニング証明書が必要です。
        * アドオン署名用の証明書を公開版に変更している場合は`git reset --hard HEAD`で製品版に戻して下さい。
        * 製品版インストーラは`dest`フォルダー配下に作成されます。
5. GitHubでリリースを作成し、上記で作成した公開版インストーラおよびGPO管理用テンプレートをアップロードします。
    *  https://github.com/FlexConfirmMail/Outlook/releases
6. flexconfirmmail.comの次のページを更新します。
    * [FlexConfirmMail > ダウンロード](https://github.com/FlexConfirmMail/flexconfirmmail.github.io/blob/main/source/download.rst)
    * [FlexConfirmMail > 開発情報](https://github.com/FlexConfirmMail/flexconfirmmail.github.io/blob/main/source/support.rst)
    * [FlexConfirmMail > サンキューページ](https://github.com/FlexConfirmMail/flexconfirmmail.github.io/blob/main/source/thankyou.rst)
    * [v22.2の例](https://github.com/FlexConfirmMail/flexconfirmmail.github.io/commit/71e007d46eaed0102c6dfb08ddf6f2567fdcc5aa)
3. リリースノートを書いて投稿します。
    * [v22.2の例](https://github.com/FlexConfirmMail/flexconfirmmail.github.io/blob/main/source/changelog/v22.2.rst)
