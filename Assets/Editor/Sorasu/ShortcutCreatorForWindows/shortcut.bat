@if(0)==(0) echo off
cscript.exe //nologo //E:JScript "%~f0" %*
goto :EOF
@end

// ���C������
function main() {
    var shortcut = null;
    try {
        // �V���[�g�J�b�g���쐬����
        shortcut = new ShortcutCreater();
        shortcut.create();

        // �쐬�����V���[�g�J�b�g���R���\�[���ɏo�͂���
        Console.println("�V���[�g�J�b�g���쐬���܂���");
        Console.println(shortcut);

    } catch (e) {

        // ��O�������R���\�[���ɏo�͂���
        Console.println("[error occured]: " + e.description);
        Console.println("usage: shortcut.bat [filepath] [linkpath]");

        // �ُ�I���ŃR�}���h��Ԃ�
        Console.back(e.number);

    } finally {

        // WSH�I�u�W�F�N�g��Еt����
        if (shortcut !== null)
            shortcut.cleanup();
    }

    // ����I���ŃR�}���h��Ԃ�
    Console.back(0);
}

// �R���\�[���ėp�N���X
var Console = ((function() {
    var constructor = function() {}
    constructor.println = echoConsole;
    constructor.back = exitScript;
    return constructor;
})())

// �V���[�g�J�b�g�����N���X
var ShortcutCreater = function() {
    var args = WScript.Arguments;
    validate(args);
    this.wshObj = openWsh();
    this.file = args(0);
    this.link = args(1);
	this.arguments = args(2);
    this.create = createShortcut;
    this.cleanup = closeWsh;
    this.toString = createrToString;
}

// ----- �ȍ~�֐��Q -------

function validate(args) {
    if (args == null) {
        throw new Error(1, "args=null or undefined");
    }
	/*
    if (args.length !== 2) {
        var str = "args = ";
        for (i=0; i < args.length; i++)
            str += "[" + i + "]:" + args(i) + " ";
        throw new Error(2, str);
    }
	*/
}

function createShortcut() {
    var lnkFile = this.wshObj.CreateShortcut(this.file);
    lnkFile.TargetPath = this.link;
	lnkFile.Arguments  = this.arguments;
    lnkFile.Save();
}

function createrToString() {
    return "file=\"" + this.file + "\", linkTo=\"" + this.link + "\"";
}

function openWsh() {
    return WScript.CreateObject("WScript.Shell");
}

function closeWsh() {
    this.wshObj = null;
}

function echoConsole(msg) {
    WScript.echo(msg);
}

function exitScript(errNum) {
    WScript.Quit(errNum);
}

// ���C�������Ăяo��
main();
cmd /k 