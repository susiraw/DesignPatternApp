using System;
using System.IO;

namespace MemoTree
{
    /// <summary>
    /// Createコマンド処理
    /// ディレクトリ、または、ファイルを作成する。
    /// </summary>
    internal class CreateCommand : Command
    {
        // Createモード
        internal Define.ComponentType m_componentType = Define.ComponentType.Dir;

        // Undo、Redoから実行した際のディレクトリ名、ファイル名
        private string m_strNoInputName;

        // エラーメッセージ
        private string m_strErrMessage = "";

        /// <summary>
        /// CreateCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            // パスを設定
            var strNewPath = "";
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
                strNewPath = base.m_component.m_strPath + "/" + context.m_strInputName;
                this.m_strNoInputName = context.m_strInputName;

            }
            else
            {
                // Redoから呼び出し
                strNewPath = base.m_component.m_strPath + "/" + this.m_strNoInputName;
            }

            switch (this.m_componentType)
            {
                case Define.ComponentType.Dir:
                    this.CreateDir(strNewPath);
                    break;
                case Define.ComponentType.File:
                    this.CreateFile(strNewPath);
                    break;
            }
        }

        /// <summary>
        /// ディレクトリを作成する
        /// </summary>
        /// <param name="strPathName"></param>
        private void CreateDir(string strPathName)
        {
            if (!Directory.Exists(strPathName))
            {
                Directory.CreateDirectory(strPathName);
                base.m_component.Add(new DirComponent(strPathName));
                // ディレクトリ配下の全ファイル名を取得
                var lstStrFileName = DirComponent.GetAllFileName((DirComponent)base.m_component);
                Common.OutputFileName(lstStrFileName);
            }
            else
            {
                this.m_strErrMessage = Define.TXT_ALREADY_EXIST;
                base.m_bStack = false;
            }
        }

        /// <summary>
        /// ファイルを作成する
        /// </summary>
        /// <param name="strPathName"></param>
        private void CreateFile(string strPathName)
        {
            strPathName = strPathName + ".txt";
            if (!File.Exists(strPathName))
            {
                File.CreateText(strPathName);
                base.m_component.Add(new FileComponent(strPathName));
                // ディレクトリ配下の全ファイル名を取得
                var lstStrFileName = DirComponent.GetAllFileName((DirComponent)base.m_component);
                Common.OutputFileName(lstStrFileName);
            }
            else
            {
                this.m_strErrMessage = Define.TXT_ALREADY_EXIST;
                base.m_bStack = false;
            }
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
        /// Undo用のCommandとしてDeleteCommandを設定する
        /// </summary>
        internal override void SetUndoCommand()
        {
            var deleteCommand = new DeleteCommand();
            var currentComponent = (DirComponent)base.m_component;
            deleteCommand.SetComponent(currentComponent.m_listComponent[currentComponent.m_listComponent.Count - 1]);
            base.m_undoCommand = deleteCommand;
        }
    }
}
