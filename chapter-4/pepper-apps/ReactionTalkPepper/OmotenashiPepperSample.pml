<?xml version="1.0" encoding="UTF-8" ?>
<Package name="OmotenashiPepperSample" format_version="4">
    <Manifest src="manifest.xml" />
    <BehaviorDescriptions>
        <BehaviorDescription name="behavior" src="." xar="behavior.xar" />
    </BehaviorDescriptions>
    <Dialogs>
        <Dialog name="SynAppsDialog" src="SynAppsDialog/SynAppsDialog.dlg" />
    </Dialogs>
    <Resources>
        <File name="azureiothub" src="lib/azureiothub.py" />
        <File name="index" src="html/index.html" />
        <File name="normalize" src="html/css/normalize.css" />
        <File name="jquery.qimhelpers2" src="html/js/jquery.qimhelpers2.js" />
        <File name="" src=".DS_Store" />
        <File name="click" src="sounds/click.ogg" />
        <File name="APJapanesefont" src="html/font/APJapanesefont.woff2" />
        <File name="animation" src="html/css/animation.css" />
        <File name="contents" src="html/css/contents.css" />
        <File name="mplus-2p-bold" src="html/font/mplus-2p-bold.woff2" />
        <File name="mplus-2p-regular" src="html/font/mplus-2p-regular.woff2" />
        <File name="balloon_friendly" src="html/img/balloon_friendly.png" />
        <File name="balloon_friendly_l" src="html/img/balloon_friendly_l.png" />
        <File name="balloon_lower" src="html/img/balloon_lower.png" />
        <File name="balloon_normal" src="html/img/balloon_normal.png" />
        <File name="balloon_play" src="html/img/balloon_play.png" />
        <File name="balloon_play_l" src="html/img/balloon_play_l.png" />
        <File name="balloon_upper" src="html/img/balloon_upper.png" />
        <File name="bg_friendly" src="html/img/bg_friendly.png" />
        <File name="bg_normal" src="html/img/bg_normal.png" />
        <File name="btn_back_normal_off" src="html/img/btn_back_normal_off.png" />
        <File name="btn_back_normal_on" src="html/img/btn_back_normal_on.png" />
        <File name="friendly_mode_title" src="html/img/friendly_mode_title.png" />
        <File name="mic_friendly_in" src="html/img/mic_friendly_in.png" />
        <File name="mic_friendly_out" src="html/img/mic_friendly_out.png" />
        <File name="mic_normal_in" src="html/img/mic_normal_in.png" />
        <File name="mic_normal_out" src="html/img/mic_normal_out.png" />
        <File name="mic_off" src="html/img/mic_off.png" />
        <File name="voice_search_icon_on" src="html/img/voice_search_icon_on.png" />
        <File name="contents" src="html/js/contents.js" />
        <File name="jquery-ui.min" src="html/js/jquery-ui.min.js" />
        <File name="btn_back_return_off" src="html/img/btn_back_return_off.png" />
        <File name="btn_back_return_on" src="html/img/btn_back_return_on.png" />
    </Resources>
    <Topics>
        <Topic name="SynAppsDialog_jpj" src="SynAppsDialog/SynAppsDialog_jpj.top" topicName="SynAppsDialog" language="ja_JP" />
    </Topics>
    <IgnoredPaths>
        <Path src=".metadata" />
    </IgnoredPaths>
    <Translations auto-fill="ja_JP">
        <Translation name="translation_en_US" src="translations/translation_en_US.ts" language="en_US" />
        <Translation name="translation_ja_JP" src="translations/translation_ja_JP.ts" language="ja_JP" />
    </Translations>
</Package>
