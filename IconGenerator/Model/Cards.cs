public class CardList
{
    public List<Card> List { get; } = [
        new Card {
            Name = "ジャーナルを陣地として見ること",
            Text = @"* ジャーナルの今日のページについて、朝のフリーライティングの存在感とinboxの存在感が拮抗してくると、心が安定してきます。気持ちが忙しくてinboxばかりになってるときは凹みます。",
            Tags = ["cardbox"]
        },
        new Card {
            Name = "スケッチについて",
            Text = @"* カード法と似ているが異なるものにスケッチがある。スケッチとは、基本的にはライティングより人工物や製品、アート作品づくりを対象とする。
  * 「表現」してみることによって、自らの知覚できる対象を広げること。表現のアプローチは複数あって、カードのほかにスケッチがある。カードとスケッチは異なるかもしれないが、似ているもののように思える。
  * エンジニアにとってのスケッチのやりかたの1つに、実際に自分の手でアプリを作ってみるということがありまして、わたしがCardBoxでいまやってるのは、これまであまり関心を向けてなかったグリッド＆フローのカードレイアウトについて改めて表現してみる作業のつもりです。",
            Tags = ["cardbox"]
        },
        new Card {
            Name = "カード法",
            Text = @"* メモ全体の整理に関する意思決定を減らすために、シンプルな構造とワークフローのシステムにする。

* 書き散らし「メモ」と永久保存版の「カード」を明確に分ける
  * Logseqではcardboxという[[ページタグ]]をつける",
            Tags = ["cardbox" , "card"]
        },
        new Card {
            Name = "カードはストーリーを伴う",
            Text = @"* カードは、取り回しのよいスライド型のコンセプトと、読み物となるストーリーの組である。
* 思考とカードとをつなぐためのワークフローをLogseq上で実施すると、この様式に収束する。
  * 始まりは、ある話題についての発想がアウトラインとして流れ出す。流れを整えたり文脈を追加して後から読み直したとき理解できるようにすると、ストーリーとして読めるものになる。
  * 長いストーリーの取り回しは悪いが、それでも1つの話題について考えたものなので、無理に複数カードへ分割するのはもったいない。
  * むしろ、長いストーリーは、発表スライドに近い形のカードへ再構築し、ストーリーとカードは組にして１つのページ内で取り扱うと良さそう。",
            Tags = ["cardbox", "card"]
        },
        new Card {
            Name = "空間とカードについて",
            Text = @"* ツールのリスト
  * MarginNote
    * https://www.marginnote.com
  * lino
    * https://ja.linoit.com
    * まだサービスやってた・・！",
            Tags = ["cardbox", "card"]
        },
        new Card {
            Name = ".NET MAUI",
            Text = @"* .NET Multi-platform App UI
  * https://learn.microsoft.com/ja-jp/dotnet/maui/?view=net-maui-8.0
*.NET MAUIの本
  *[Free Ebook - .NET MAUI Community Toolkit Succinctly (syncfusion.com)](https://www.syncfusion.com/succinctly-free-ebooks/dotnet-maui-community-toolkit-succinctly?utm_source=alvinashcraft&utm_medium=email&utm_campaign=alvinashcraft_blog_edmmar24)",
            Tags = ["framework"]
        },
        new Card {
            Name = "Avalonia UI",
            Text = @"* Avalonia UIの概要
  * WPFの流儀を引き継いだマルチプラットフォームUIフレームワーク
    * 移植ではなく概念を引き継いだもの。
    * 例えば、複数スタイルの適用はWPFでは based on で派生させるしかないが、Avaloniaであれば Classes=""h1 blue""と書ける。",
            Tags = ["framework"]
        },
        new Card {
            Name = "WPF",
            Text = @"* Windows Presentation Foundation
* Win32(GDI)ベースのフレームワーク（Windows フォーム）では、ウィンドウ内にあるボタンなどのコントロールもウィンドウ（子ウィンドウ）であり、それぞれ Windowハンドル(hWnd)を持っている。
* WPFのコントロールはウィンドウではないため、WPFではアプリケーションのトップレベルのウィンドウしか Windowハンドルを取得することはできない。",
            Tags = ["framework"]
        },
        new Card {
            Name = "ドキュメントデータベース",
            Text = @"* データ構造について
  * 【構造】[半構造化データとドキュメントデータベース](https://scrapbox.io/sosuisen/%E5%8D%8A%E6%A7%8B%E9%80%A0%E5%8C%96%E3%83%87%E3%83%BC%E3%82%BF%E3%81%A8%E3%83%89%E3%82%AD%E3%83%A5%E3%83%A1%E3%83%B3%E3%83%88%E3%83%87%E3%83%BC%E3%82%BF%E3%83%99%E3%83%BC%E3%82%B9) (#scrapbox)
  * 【整合性】１つのドキュメント内で整合性を完結させるのを原則としたほうがよい",
            Tags = []
        },
        new Card {
            Name = "MongoDB",
            Text = @"* https://www.mongodb.com
* https://www.fenet.jp/infla/column/technology/mongodb%E3%81%AE%E5%88%A9%E7%82%B98%E3%81%A4%EF%BD%9Cmongodb%E3%81%AE%E6%A6%82%E8%A6%81%E3%82%84%E7%89%B9%E5%BE%B4%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6%E3%82%82%E8%A7%A3%E8%AA%AC/
* テーブルに相当するコレクションがある。
  * コレクションごとにドキュメントを格納。
  * コレクションごとにドキュメントを find()",
            Tags = []
        },
    ];
}

public class Card(string name = "", string text = "", List<String>? tags = null){
    public string Name { get; set; } = name;
    public string Text { get; set; } = text;
    public List<String> Tags { get; set; } = tags ?? [];
}
