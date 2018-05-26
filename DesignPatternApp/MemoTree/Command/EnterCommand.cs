using System;
using System.Collections.Generic;

namespace MemoTree
{
    /// <summary>
    /// Enterコマンド処理
    /// 対象がディレクトリの場合、配下のファイル名を表示する。
    /// 対象がファイルの場合、対象ファイルを開く。
    /// </summary>
    public class EnterCommand : Command
    {
        /// <summary>
        /// EnterCommandの処理を実行する
        /// </summary>
        /// <param name="context"></param>
        internal override void Execute(Context context)
        {
            if (base.m_component.GetType() == typeof(DirComponent))
            {
                // ディレクトリ配下の全ファイル名を取得
                var lstStrFileName = DirComponent.GetAllFileName((DirComponent)base.m_component);
                OutputFileName(lstStrFileName);
            }
            else
            {
                Common.FileOpen(base.m_component.m_strPath);
            }
        }

        /// <summary>
        /// Undo用のCommandとしてEnterCommandを設定する
        /// </summary>
        internal override void SetUndoCommand()
        {
            // TODO Undo用のCommandとしてEnterCommandを設定する
        }

        /// <summary>
        /// ファイル名の先頭に、0からの番号を付与して出力を行う
        /// </summary>
        /// <param name="lstStrFileName"></param>
        private static void OutputFileName(List<string> lstStrFileName)
        {
            Common.InitConsole();

            var iIdx = 0;
            foreach (var strfileName in lstStrFileName)
            {
                Console.WriteLine(iIdx + " : " + strfileName);
                iIdx++;
            }
        }
    }
}
