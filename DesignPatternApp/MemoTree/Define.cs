using System;
namespace MemoTree
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    internal static class Define
    {
        /// <summary>
        /// コンポーネントのタイプ（Create、Edit）
        /// </summary>
        internal enum ComponentType : byte
        {
            Dir = 0,
            File
        }

        // TODO 未使用
        // 最大階層
        internal static readonly int MAX_HIERARCHY_NUM = 5;
        // 一階層あたりの最大ファイル数
        internal static readonly int MAX_COMPONENT_NUM = 10;

        /// <summary>
        /// デフォルトテキスト
        /// </summary>
        internal static readonly string TXT_DEFAULT =
            "----------------------------------------------------------" + Environment.NewLine +
            "shift + C : Create directory or file" + Environment.NewLine +
            "shift + Z : Undo" + Environment.NewLine +
            "shift + Y : Redo" + Environment.NewLine +
            "0~9 + return : Open directory or file" + Environment.NewLine +
            "z + return : Return before directory" + Environment.NewLine +
            "0~9 + E : Edit directory name or file name" + Environment.NewLine +
            "0~9 + D : Delete directory or file ※Delete can't Undo" + Environment.NewLine +
            "Escape : End this application" + Environment.NewLine +
            "----------------------------------------------------------";

        /// <summary>
        /// Create、Edit時に既に同一のファイル名が存在
        /// </summary>
        internal static readonly string TXT_ALREADY_EXIST = "This name is already exist";
    }
}
