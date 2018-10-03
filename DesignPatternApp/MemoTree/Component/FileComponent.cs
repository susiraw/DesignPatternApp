using System;
using System.IO;

namespace MemoTree
{
    /// <summary>
    /// ファイルコンポーネント
    /// </summary>
    internal class FileComponent : Component
    {
        /// <summary>
        /// コンストラクタ
        /// 各メンバーの設定を行う
        /// </summary>
        /// <param name="strPath"></param>
        internal FileComponent(string strPath)
        {
            this.m_strPath = strPath;
            this.m_strName = Path.GetFileName(strPath);
        }
    }
}
