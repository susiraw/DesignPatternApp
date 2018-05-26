using System.IO;

namespace MemoTree
{
    /// <summary>
    /// Createコマンド処理
    /// ディレクトリ、または、ファイルを作成する。
    /// </summary>
    public class CreateCommand : Command
    {
        /// <summary>
        /// Createモード
        /// </summary>
        internal enum CreateMode : byte
        {
            CreateDir = 0,
            CreateFile
        }

        // TODO プロパティ化の検討
        // Createモード
        internal CreateMode m_createMode = CreateMode.CreateDir;

        /// <summary>
        /// CreateCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            // 作成対象となるパスを設定
            var strPathName = Path.GetDirectoryName(base.m_component.m_strPath) + "/" + base.m_component.m_strName;
            switch (this.m_createMode)
            {
                case CreateMode.CreateDir:
                    CreateDir(strPathName);
                    break;
                case CreateMode.CreateFile:
                    CreateFile(strPathName);
                    break;
            }
        }

        /// <summary>
        /// ディレクトリを作成する
        /// </summary>
        /// <param name="strPathName"></param>
        private static void CreateDir(string strPathName)
        {
            if (!Directory.Exists(strPathName))
            {
                Directory.CreateDirectory(strPathName);
            }
            // TODO 既に存在する場合の処理を検討
        }

        /// <summary>
        /// ファイルを作成する
        /// </summary>
        /// <param name="strPathName"></param>
        private void CreateFile(string strPathName)
        {
            if (!File.Exists(strPathName))
            {
                File.CreateText(strPathName);
            }
            // TODO 既に存在する場合の処理を検討
        }

        /// <summary>
        /// Undo用のCommandとしてDeleteCommandを設定する
        /// </summary>
        internal override void SetUndoCommand()
        {
            // TODO Undo用のCommandとしてDeleteCommandを設定する
        }
    }
}
