using System;
namespace MemoTree
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    public static class Define
    {
        // 最大階層
        internal static readonly int MAX_HIERARCHY_NUM = 5;
        // 一階層あたりの最大ファイル数
        internal static readonly int MAX_COMPONENT_NUM_OF_ONE_HIERARCHY = 10;


        // TODO デフォルトテキストを考える

        // デフォルトテキスト
        internal static readonly string DEFAULT_TXET =
            "shift + C Create directory or file, shift + Z Undo, shift + Y Redo" + Environment.NewLine +
            "0~9 + return Open directory or file, 0~9 + E Edit directory or file, 0~9 + D Delete directory or file" + Environment.NewLine +
            "Escape End this application" + Environment.NewLine +
            "------------------------------------------------------------------------------------------";
    }
}
