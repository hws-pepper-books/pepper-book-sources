# pepper-book-sources

## はじめに
本アプリケーションは、Microsoft Azure Platform上での稼働を前提としています。<br/>
本アプリケーションの動作には、下記のフレームワークおよびサービスが必要です。<br/>
Microsoft Azure Platformを利用するためには、Microsoftアカウントの取得が必要です。<br/>
また、利用するサービスに応じた料金が発生します。

・Microsoft Cognitive Services LUIS

https://azure.microsoft.com/ja-jp/services/cognitive-services/language-understanding-intelligent-service/
https://www.luis.ai/

・Microsoft Cognitive Services Face API

https://azure.microsoft.com/ja-jp/services/cognitive-services/face/

・Cloud Robotics Azure Platform

https://github.com/seijim/cloud-robotics-azure-platform-v1-sdk

※Cloud Robotics Azure Platformとは、Azure IoT Hub を対象にしたクラウドサービス ベースのアプリケーション サーバーです。<br/>
本アプリケーションは、Cloud Robotics Azure Platform上で動作するdllとして実装されています。


## 利用方法
### CRFX Dllアプリケーション
Cloud Robotics Azure Platformと同一のSQL Database上にサーバアプリケーション実行用のテーブルを作成します。<br/>
テーブル作成には、リポジトリの下記ディレクトリに配備されているSQLを実行してください。

crfx-appdll/cloud-robotics-fx-hws-reaction-for-books/db<br/>
crfx-appdll/cloud-robotics-fx-hws-face-books/db

※下記SQLには、実行環境に応じたidおよびkey等の情報を設定する必要があります。<br/>
SQL実行前にご利用になられる環境に合わせて設定してください。

crfx-appdll/cloud-robotics-fx-hws-reaction-for-books/db/09_RegisterAppMaster.sql<br/>
crfx-appdll/cloud-robotics-fx-hws-face-books/db/03_RegisterAppMaster.sql

### LUISアプリケーション

LUIS: Homepage (https://www.luis.ai/) 
の「My Apps」にて「Import new app」を選択し、
リポジトリの下記ディレクトリに配備されているJSONを選択してください。

crfx-appdll/cloud-robotics-fx-hws-reaction-for-books/luis-app

アプリケーションが作成されたら、「Train」と「Publish」を実行することでLUS API呼び出しが、Pepperアプリケーション<br/>
pepper-apps/ReactionTalkPepper/OmotenashiPepperSample.pmlを起動します。

下記ボックスに実行環境に応じたidおよびkey等の情報を設定してください。<br/>
root/Init/AppSettings

## 免責事項

本アプリケーションは、利用者の学習目的に構築されたものであり、MITライセンスとして提供されます。<br/>
本アプリケーションを利用することにより発生したいかなる損害に対しても責任を負うものではない旨、あらかじめご了承ください。
