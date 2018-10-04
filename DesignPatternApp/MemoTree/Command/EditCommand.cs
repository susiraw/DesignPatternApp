using System;
using System.IO;

namespace MemoTree
{
    /// <summary>
    /// Editコマンド処理
    /// ディレクトリ、または、ファイルの名前を変更する。
    /// </summary>
    internal class EditCommand : Command
    {
        // Undo、Redoから実行した際のディレクトリ名、ファイル名
        private string m_strNoInputName;
        // Undo設定用にディレクトリ名、ファイル名を保持
        private string m_strTmpBeforeName;

        // エラーメッセージ
        private string m_strErrMessage = "";

        /// <summary>
        /// EditCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            // Undo用に編集前の名前を保持
            this.m_strTmpBeforeName = base.m_component.m_strName;
            // パス、名前を設定
            var strNewPath = "";
            var strNewName = "";
            if (context.m_strInputName != null)
            {
                // 未入力の場合、処理を実行しない
                if (context.m_strInputName == "")
                {
                    this.m_strErrMessage = Define.TXT_INPUT_EMPTY;
                    base.m_bStack = false;
                    return;
                }
                // 入力あり
                strNewPath = Path.GetDirectoryName(base.m_component.m_strPath) + "/" + context.m_strInputName;
                strNewName = context.m_strInputName;
                this.m_strNoInputName = context.m_strInputName;
            }
            else
            {
                // Undo、Redoから呼び出し
                strNewPath = Path.GetDirectoryName(base.m_component.m_strPath) + "/" + this.m_strNoInputName;
                strNewName = this.m_strNoInputName;
            }

            // Undo用に編集前の名前を保持
            if (base.m_component.GetType() == typeof(DirComponent))
            {
                this.EditDirName(strNewName, base.m_component.m_strPath, strNewPath);
            }
            else
            {
                this.EditFileName(strNewName, base.m_component.m_strPath, strNewPath);
            }
        }

        /// <summary>
        /// ディレクトリ名を編集する
        /// </summary>
        /// <param name="strPathNameOld"></param>
        /// <param name="strPathNameNew"></param>
        private void EditDirName(string strNewName, string strPathNameOld, string strPathNameNew)
        {
            if (!Directory.Exists(strPathNameNew))
            {
                Directory.Move(strPathNameOld, strPathNameNew);
                ModifyPath(this.m_component, strPathNameOld, strPathNameNew);
                this.m_component.m_strName = strNewName;
            }
            else
            {
                this.m_strErrMessage = Define.TXT_ALREADY_EXIST;
                base.m_bStack = false;
            }
        }

        /// <summary>
        /// ファイル名を編集する
        /// </summary>
        /// <param name="strPathNameOld"></param>
        /// <param name="strPathNameNew"></param>
        private void EditFileName(string strNewName, string strPathNameOld, string strPathNameNew)
        {
            if (!strNewName.EndsWith(".txt"))
            {
                strNewName = strNewName + ".txt";
                strPathNameNew = strPathNameNew + ".txt";
            }

            if (!File.Exists(strPathNameNew))
            {
                File.Move(strPathNameOld, strPathNameNew);
                ModifyPath(this.m_component, strPathNameOld, strPathNameNew);
                this.m_component.m_strName = strNewName;
            }
            else
            {
                this.m_strErrMessage = Define.TXT_ALREADY_EXIST;
                base.m_bStack = false;
            }
        }

        /// <summary>
        /// 編集後の名称にてコンポーネントのパスを再帰的に修正する
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="strPathNameOld">String path name old.</param>
        /// <param name="strPathNameNew">String path name new.</param>
        private static void ModifyPath(Component component, string strPathNameOld, string strPathNameNew)
        {
            if (component.GetType() == typeof(DirComponent))
            {
                foreach (var subComponent in ((DirComponent)component).m_listComponent)
                {
                    ModifyPath(subComponent, strPathNameOld, strPathNameNew);
                }
            }
            component.m_strPath = component.m_strPath.Replace(strPathNameOld, strPathNameNew);
        }

        /// <summary>
        /// 各Commandの実行後の処理を行う
        /// </summary>
        /// <param name="context">Context.</param>
        internal override void AfterExecute(Context context, bool bClearRedoStack = true)
        {
            base.AfterExecute(context, bClearRedoStack);
            if (this.m_strErrMessage != "")
            {
                Console.WriteLine(this.m_strErrMessage);
                this.m_strErrMessage = "";
            }
        }

        /// <summary>
        /// Undo用のCommandとしてEditCommandを設定する
        /// </summary>
        internal override void SetUndoCommand()
        {
            var editCommand = new EditCommand();
            editCommand.SetComponent(base.m_component);
            editCommand.m_strNoInputName = this.m_strTmpBeforeName;
            base.m_undoCommand = editCommand;
        }
    }
}
