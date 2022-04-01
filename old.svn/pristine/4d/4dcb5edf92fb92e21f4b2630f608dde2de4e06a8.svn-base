using System;
using RazorEngine;
using RazorEngine.Templating;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //var body = TemplateLib.Razor.RunCompile(LINE_REQ_MESSAGE_TEMPLATE, RAZOR_TEMPLATE_KEY_REQ, new
            //{
            //    SystemName = "XXシステム",
            //    Url = "http://metamedia.co.jp?id=123"
            //});
            //Console.WriteLine(body);

            var model = new
            {
                SystemName = "XXシステム",
                Url = "http://metamedia.co.jp?id=123"
            };
            var body =
            $@"このメールは{model.SystemName} からのLINEアカウント連携リクエストメールです

状態変動時の通知をLINEで受信する場合は以下のURLをクリックしてください
{model.Url}

LINEによるメッセージ受信には以下の条件が必要です。

1.LINEを現在利用している(使用を開始する)
2.{model.SystemName}公式アカウントをお友達に追加している

LINEはスマートフォンで以下のURLにアクセスいただき
無料でインストール可能です

Google Play(Android)
https://play.google.com/store/apps/details?id=jp.naver.line.android

App Store(iPhone/ iPad)
https://apps.apple.com/jp/app/line/id443904275


本メールに心当たりのない方はお手数ですが以下のアドレスまでご連絡願います。
mailto: admin @metamedia.co.jp

------------------------------------------------

{model.SystemName}事務局

------------------------------------------------
";

            Console.WriteLine(body);

        }
        const string RAZOR_TEMPLATE_KEY_REQ = "lineRequestTemplate";


        const string LINE_REQ_MESSAGE_TEMPLATE =
@"このメールは@(Model.SystemName) からのLINEアカウント連携リクエストメールです

状態変動時の通知をLINEで受信する場合は以下のURLをクリックしてください
@Raw(Model.Url)

LINEによるメッセージ受信には以下の条件が必要です。

1.LINEを現在利用している(使用を開始する)
2.@(Model.SystemName)公式アカウントをお友達に追加している

LINEはスマートフォンで以下のURLにアクセスいただき
無料でインストール可能です

Google Play(Android)
https://play.google.com/store/apps/details?id=jp.naver.line.android

App Store(iPhone/iPad)
https://apps.apple.com/jp/app/line/id443904275


本メールに心当たりのない方はお手数ですが以下のアドレスまでご連絡願います。
mailto:admin@metamedia.co.jp

------------------------------------------------

@(Model.SystemName)事務局

------------------------------------------------
";

    }
}
