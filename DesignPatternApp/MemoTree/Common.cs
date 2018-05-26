using System;
using System.Collections.Generic;
using System.IO;

namespace MemoTree
{
    /// <summary>
    /// 共通処理クラス
    /// </summary>
    public static class Common
    {
        // TODO 未使用
        /// <summary>
        /// 指定されたパスに存在するファイル名の一覧を取得する。
        /// </summary>
        /// <returns>ファイル名の一覧</returns>
        /// <param name="strPath">指定パス</param>
        public static List<string> GetAllFileName(string strPath)
        {
            var astrDirectoryName = Directory.GetDirectories(strPath);
            var astrFileName = Directory.GetFiles(strPath, "*.txt");

            var lstFileName = new List<string>();
            lstFileName.AddRange(astrDirectoryName);
            lstFileName.AddRange(astrFileName);

            return lstFileName;
        }

        /// <summary>
        /// 指定されたパスのファイルを開く。
        /// </summary>
        /// <param name="strPath">指定パス</param>
        public static void FileOpen(string strPath)
        {
            System.Diagnostics.Process.Start(strPath);
        }

        /// <summary>
        /// コンソールの初期表示
        /// </summary>
        public static void InitConsole()
        {
            Console.Clear();
            Console.WriteLine(Define.DEFAULT_TXET);
        }

        // TODO コンソールの再描画処理
    }
}
