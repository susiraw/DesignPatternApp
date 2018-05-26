using System.IO;

namespace MemoTree
{
    /// <summary>
    /// ファイルコンポーネント
    /// </summary>
    public class FileComponent : Component
    {
        // TODO ファイル内容を持つか検討

        /// <summary>
        /// コンストラクタ
        /// 各メンバーの設定を行う
        /// </summary>
        /// <param name="strPath"></param>
        public FileComponent(string strPath)
        {
            this.m_strPath = strPath;
            this.m_strName = Path.GetFileName(strPath);
        }
    }
}
