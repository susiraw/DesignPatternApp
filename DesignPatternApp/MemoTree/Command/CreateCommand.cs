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

        /// <summary>
        /// CreateCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            // 作成対象となるパスを設定
            var strPathName = base.m_component.m_strPath + "/" + context.m_strInputName;
            switch (this.m_componentType)
            {
                case Define.ComponentType.Dir:
                    this.CreateDir(strPathName);
                    break;
                case Define.ComponentType.File:
                    this.CreateFile(strPathName);
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
                Console.WriteLine(Define.TXT_ALREADY_EXIST);
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
                Console.WriteLine(Define.TXT_ALREADY_EXIST);
                base.m_bStack = false;
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
