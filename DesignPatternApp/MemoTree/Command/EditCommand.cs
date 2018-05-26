using System.IO;

namespace MemoTree
{
    /// <summary>
    /// Editコマンド処理
    /// ディレクトリ、または、ファイルの名前を変更する。
    /// </summary>
    public class EditCommand : Command
    {
        /// <summary>
        /// Editモード
        /// </summary>
        internal enum EditMode : byte
        {
            EditDirName = 0,
            EditFileName
        }

        // TODO プロパティ化の検討
        // Editモード
        internal EditMode m_editMode = EditMode.EditDirName;

        /// <summary>
        /// EditCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            // 編集対象となるパスを設定
            var strPathName = Path.GetDirectoryName(base.m_component.m_strPath) + "/" + base.m_component.m_strName;
            switch (this.m_editMode)
            {
                case EditMode.EditDirName:
                    EditDirName(base.m_component.m_strPath, strPathName);
                    break;
                case EditMode.EditFileName:
                    EditFileName(base.m_component.m_strPath, strPathName);
                    break;
            }
        }

        /// <summary>
        /// ディレクトリ名を編集する
        /// </summary>
        /// <param name="strPathNameOld"></param>
        /// <param name="strPathNameNew"></param>
        private static void EditDirName(string strPathNameOld, string strPathNameNew)
        {
            if (!Directory.Exists(strPathNameNew))
            {
                Directory.Move(strPathNameOld, strPathNameNew);
            }
            // TODO 既に存在する場合の処理を検討
            // TODO 編集後のディレクトリ名に合わせてコンポーネントを再帰的に書き換える
            // TODO ディレクトリ配下の再取得の方が実装コストが低い
        }

        /// <summary>
        /// ファイル名を編集する
        /// </summary>
        /// <param name="strPathNameOld"></param>
        /// <param name="strPathNameNew"></param>
        private static void EditFileName(string strPathNameOld, string strPathNameNew)
        {
            if (!File.Exists(strPathNameNew))
            {
                File.Move(strPathNameOld, strPathNameNew);
            }
            // TODO 既に存在する場合の処理を検討
        }

        /// <summary>
        /// Undo用のCommandとしてEditCommandを設定する
        /// </summary>
        internal override void SetUndoCommand()
        {
            // TODO Undo用のCommandとしてEditCommandを設定する
        }
    }
}
