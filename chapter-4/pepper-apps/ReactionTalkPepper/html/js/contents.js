var PEPPER = PEPPER || {}

var pepper_onStart = function() {

    var TOUCH = (window.ontouchstart === null),
    START = TOUCH ? 'touchstart' : 'mousedown',
    MOVE = TOUCH ? 'touchmove' : 'mousemove',
    END = TOUCH ? 'touchend' : 'mouseup',
    JP = 'jp', EN = 'en';

    var textList = [];
    var textNum = 0;

    var BALLOON_COLORS = ["pink", "green", "orange", "blue", "red"];
    var enableSelectFlg = false;
    var startBalloonTimer;
    var selectedEl = null;

    // シャッフル関数
    var shuffleArray = function(array) {
        var n = array.length, t, i;

        while (n) {
            i = Math.floor(Math.random() * n--);
            t = array[n];
            array[n] = array[i];
            array[i] = t;
        }

        return array;
    }

    // 画面切り替え
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/ImageChange", function (data) {
        $('.page').hide();
        $('#filter').hide();
        if(data=="solitary"){
            initBalloon();
            $("#normal_btn_top").css('background-image', $("#normal_btn_top").css('background-image').replace('_on.png', '_off.png'));
        }else if(data=="normal_mode"){
            $('#' + data +'_microphone_icon_on').hide();
            $('#' + data +'_microphone_icon_off').hide();
            $('#' + data +' .pepper_balloon').hide();
            $('#' + data +' .cuntomer_balloon').hide();
        }

        $('#' + data).show();
    });

    // dialog起動中は、ヘッダー以外は使用不可
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/ShowFilterDialog", function (data) {
        $('#filter_dialog').show();
    });
    // dialog起動中は、ヘッダー以外は使用不可
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/HideFilterDialog", function (data) {
        $('#filter_dialog').hide();
    });

    // タイムアウト
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Timeout", function (data) {
        $('#filter').show();
    });

    // balloonアニメーション開始
    var startBalloonAnim = function(balloonIndex) {
        if(balloonIndex < $(".balloon").length) {
            startBalloonTimer = setTimeout(function() {
                $(".balloon").eq(balloonIndex).show().addClass("balloon-anim"+(balloonIndex+1));
                startBalloonAnim(++balloonIndex);
            }, 800);
        }
    }

    // balloonの表示開始
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/StartBalloon", function (data) {
        enableSelectFlg = true;
        startBalloonAnim(0);
    });

    // balloon初期化
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/InitBalloon", function (data) {
        initBalloon();
    });

    var initBalloon = function(data) {
        $(".balloon").removeClass("select-anim").each(function(i, el) {
            $(el).css("opacity", 0).removeClass("balloon-anim"+(i+1));
        }).hide();
        $(".balloon-voice.normal").removeClass("select-voice-anim");
        $(".balloon-voice.play_setting").removeClass("select-voice-anim-fixed");
        $("#balloon-color").removeClass().addClass("balloon-box");
        $("#balloon-modal").hide().removeClass("balloon-modal");
    }

    // balloon内のテキスト設定
    var setBalloonText = function(el) {
        if(textList.length - 1 < textNum) textNum = 0;
        $(el).find("p").text(textList[textNum]);
        textNum++;
    }

    // バルーンへの文言表示
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/BalloonWord", function (data) {
        // 取得データをセット
        textList = shuffleArray(data);
        console.log(textList);
        for(var i = 1; i <= $(".balloon").length -2; i++) {
            setBalloonText($(".balloon"+i));
        }
    });

    // バルーンを音声で選択
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/SelectedVoice", function (data) {
        if(!enableSelectFlg) return;
        enableSelectFlg = false;
        clearTimeout(startBalloonTimer);
        
        // cssアニメーション初期化
        $(".balloon-voice.normal").removeClass("select-voice-anim");
        $(".balloon-voice.play_setting").removeClass("select-voice-anim-fixed");

        var balloonText = data;
        $("#balloon-modal").show().addClass("balloon-modal");
        var fixedType = ".normal";
        if(data == "ペッパーとあそぶ"){
            fixedType = ".play_setting";
        }
        var fixedName = ""; 
        if(data == "ペッパーとあそぶ"){
            fixedName = "-fixed";
            balloonText = "";
        }else{
            color = BALLOON_COLORS[Math.floor(Math.random() * BALLOON_COLORS.length)];
            $("#balloon-color").addClass(color);
        }

        $(".balloon-voice" + fixedType).addClass("select-voice-anim" + fixedName).one("webkitAnimationEnd", function() {
            $(".balloon").each(function(i, el) {
                $(el).css("opacity", 0).removeClass("balloon-anim"+(i+1));
            });
        }).find("p").text(balloonText);
        selectedEl = $(".balloon-voice" + fixedType);

        $.raiseALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/SelectedBalloon", data);
    });

    // balloonをタブレットで選択
    $(".balloon").on("touchstart", function() {
        if(!enableSelectFlg || event.touches.length != 1) return;
        enableSelectFlg = false;
        clearTimeout(startBalloonTimer);

        var selectEl = $(this);
        selectedEl = selectEl;
        var selectIndex = $(".balloon").index(selectEl);
        var balloonText = selectEl.find("p").text();
        if(selectEl.hasClass("play")){
            balloonText = "ペッパーとあそぶ";
        }

        $("#balloon-modal").show().addClass("balloon-modal");
        AudioPlayer.play('click.ogg');

        selectEl.css("opacity", 1).removeClass("balloon-anim"+(selectIndex+1));
        var fixedName = ""; 
        if(selectEl.hasClass("play")){
            fixedName = "-fixed";
        }
        selectEl.addClass("select-anim" + fixedName).one("webkitAnimationEnd", function() {
            $(".balloon").each(function(i, el) {
                if(selectEl != $(el)){
                    $(el).css("o  pacity", 0).removeClass("balloon-anim"+(i+1));
                }
            });
        });
        
        $.raiseALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/SelectedBalloon", balloonText);
    });

    // balloonが消えた後にテキストを入れ替える
    $(".balloon").on("webkitAnimationIteration", function() {
        if($(this).hasClass('play')){
            return;
        }
        setBalloonText($(this));
    });

    // balloonをfadeout
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Fadeout/Balloon", function (data) {
        var fixedName = "";
        if(data){
            fixedName = "-fixed";
        }
        selectedEl.removeClass("select-anim" + fixedName);
        selectedEl.addClass("fadeout-anim" + fixedName).one("webkitAnimationEnd", function() {
            $.raiseALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/SelectedWordAnimEnd", "");
            selectedEl.removeClass("fadeout-anim" + fixedName);
        });        
    });

    // 戻るを選択
    $('#normal_btn_top').on({
        'touchstart': function () {
            $('#filter').show();
            var url = $(this).css('background-image').replace('_off.png', '_on.png');
            $(this).css('background-image', url);
            AudioPlayer.play('click.ogg');
            console.log('touchstart');
        },
        'touchend': function () {
            console.log('touchend');
            $.raiseALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/TopTouched", 0);
        }
    });    

    // Pepperの発話文表示
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Show/PepperSay", function (data) {
        var el = "#" + data[0] + " .pepper_balloon";
        var text = data[1];
        $(el).removeClass("pushUp");
        $(el).removeClass("pullDown");
        $(el).find("p").text(text);
        $(el).show().addClass("pullDown");
    });
    // Pepperの発話文非表示
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Hide/PepperSay", function (data) {
        var el = "#" + data + " .pepper_balloon";

        $(el).removeClass("pullDown");
        $(el).addClass("pushUp").one("webkitAnimationEnd", function() {
            $(el).hide();
            $(el).find("p").text("");
        });
    });

    // お客の発話文表示
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Show/CustomerSay", function (data) {
        var el = "#" + data[0] + " .cuntomer_balloon";
        var text = data[1];
        $(el).removeClass("pushDown");
        $(el).removeClass("pullUp");
        $(el).find("p").text(text);
        $(el).show().addClass("pullUp");
    });
    // お客の発話文非表示
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Hide/CustomerSay", function (data) {
        console.log("HideCustomerSay");
        console.log(data);
        var el = "#" + data + " .cuntomer_balloon";
        $(el).removeClass("pullUp");
        $(el).addClass("pushDown").one("webkitAnimationEnd", function() {
            $(el).hide();
            $(el).find("p").text("");
        });
    });

    // 聞き取り開始
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Show/Microphone", function (data) {
        var el = "#" + data + "_microphone_icon";
        console.log(el + "_off")
        $(el + "_off").hide();
        $(el + "_on").show();
    });
    // 聞き取り終了
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Hide/Microphone", function (data) {
        var el = "#" + data + "_microphone_icon";
        console.log(el)
        $(el + "_on").hide();
        $(el + "_off").show();
    });

    // Bizメニューへ戻る
    var time_key = null;
    $('#solitary_microphone_icon').on({
      touchstart: function () {
        time_key = setTimeout(function () {
          AudioPlayer.play('click.ogg');
          $.raiseALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Exit", 1);
          time_key = null;
        }, 3000);
      },
      touchend: function () {
        if (time_key) {
          clearTimeout(time_key);
          time_key = null;
        }
      }
    });

    // ロード完了数を数えて関数を実行するクロージャー
    var ImgLoader = function(num, callback) {
        var count = 0;
        return function() {
            if(++count >= num) {
                callback();
            }
        };
    }

    // 画像プリロード
    $.subscribeToALMemoryEvent("Headwaters/ReactionTalkPepper/Main/Tablet/Preload", function (data) {
        var images = data.split(",");
        var loader = ImgLoader(images.length, function(){
            $.raiseALMemoryEvent("startYApp", "");
        });

        // 画像プリロード
        $.each(images, function(i, image) {
            var img = new Image();
            $("body").append(img);
            img.src = "img/" + image;
            img.width = 0;
            img.height = 0;
            img.onload = loader;
        });
    });

};
