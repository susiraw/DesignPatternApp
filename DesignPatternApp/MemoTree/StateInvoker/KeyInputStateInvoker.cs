using System;
using System.Collections.Generic;

namespace MemoTree
{
    /// <summary>
    /// キー入力モードのInvoker
    /// </summary>
    public class KeyInputStateInvoker : StateInvoker
    {
        #region Singleton
        /// <summary>
        /// Singletonパターンにて実装
        /// </summary>
        private static StateInvoker stateInvoker;

        /// <summary>
        /// Singletonパターンにて実装
        /// </summary>
		private KeyInputStateInvoker()
        {
        }

        /// <summary>
        /// Singletonパターンにて実装
        /// </summary>
        /// <returns></returns>
        public static StateInvoker GetInstance()
        {
            if (stateInvoker == null)
            {
                stateInvoker = new KeyInputStateInvoker();
            }
            return stateInvoker;
        }
        #endregion

        // Undo用のコマンドを保持
        private Stack<Command> m_undoStack = new Stack<Command>();
        // Redo用のコマンドを保持
        private Stack<Command> m_redoStack = new Stack<Command>();

        /// <summary>
        /// キー入力モードの処理を行う
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true:処理の続行、false:処理の終了</returns>
        public override bool Execute(Context context)
        {
            while (true)
            {
                // Key入力を受け付ける
                var keyInfo = Console.ReadKey(true);
                var iNumericKeyNo = 0;
                if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                {
                    // Shift + Key
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.C:
                            // Create処理（ディレクトリ、ファイル）
                            this.SetCharInputStateInvoker(context, true);
                            return true;
                        case ConsoleKey.Z:
                            // TODO Redo処理
                            // Redo処理
                            Console.WriteLine("Redo Key");
                            break;
                        case ConsoleKey.Y:
                            // TODO Undo処理
                            // Undo処理
                            Console.WriteLine("Undo Key");
                            break;
                    }
                }
                else if (IsNumericKey(keyInfo.Key, ref iNumericKeyNo))
                {
                    // 数値（0〜9） + Key
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Enter:
                            // TODO Enter処理
                            // Enter処理
                            Console.WriteLine("Enter Key");
                            break;
                        case ConsoleKey.D:
                            // TODO delete処理
                            // delete処理
                            Console.WriteLine("Delete Key");
                            break;
                        case ConsoleKey.E:
                            // Edit処理
                            this.SetCharInputStateInvoker(context, false, iNumericKeyNo);
                            return true;
                    }
                }
                else
                {
                    switch (keyInfo.Key)
                    {

                        case ConsoleKey.Escape:
                            return false;
                        default:
                            // TODO 不要
                            Console.WriteLine("その他" + keyInfo.Key);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 文字入力モードの設定を行う。
        /// </summary>
        /// <param name="bIsCreateCommand">true:CreateCommand、false:EditCommand</param>
        /// <param name="context"></param>
        /// <param name="iNumericKeyNo"></param>
        private void SetCharInputStateInvoker(Context context, bool bIsCreateCommand, int iNumericKeyNo = 0)
        {
            // Key入力モードのインスタンスを取得
            var charInputStateInvoker = (CharInputStateInvoker)CharInputStateInvoker.GetInstance();
            if (bIsCreateCommand)
            {
                // Createコマンドを生成し、KeyStateInvoker、CharStateInvokerに設定
                Console.WriteLine("D create directory, F create file");
                var createCommand = new CreateCommand();
                DirComponent.GetComponent(context.m_componentMain, context.m_currentDirectory);
                context.m_stateInvoker.SetCommand(createCommand);
                charInputStateInvoker.SetCommand(createCommand);
            }
            else
            {
                // Editコマンドを生成し、KeyStateInvoker、CharStateInvokerに設定
                Console.WriteLine("D edit directory name, F edit file name");
                var editCommand = new EditCommand();
                // TODO Editコマンド実行時のコンポーネントの設定
                context.m_stateInvoker.SetCommand(editCommand);
                charInputStateInvoker.SetCommand(editCommand);
            }

            var bContinue = true;
            while (bContinue)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D:
                        // TODO Createモードの設定　Command側にてComponentのタイプで判定するなら不要
                        // CharStateInvokerの処理モードをDirectoryに設定
                        //charInputStateInvoker.m_mode = 
                        //bIsCreateCommand ? CharInputStateInvoker.Mode.CreateDir : CharInputStateInvoker.Mode.EditDirName;
                        bContinue = false;
                        break;
                    case ConsoleKey.F:
                        // TODO Editモードの設定　Command側にてComponentのタイプで判定するなら不要
                        // CharStateInvokerの処理モードをFileに設定
                        //charInputStateInvoker.m_mode = 
                        //bIsCreateCommand ? CharInputStateInvoker.Mode.CreateFile : CharInputStateInvoker.Mode.EditFileName;
                        bContinue = false;
                        break;
                    default:
                        Console.WriteLine("Please press D or F key.");
                        break;
                }
            }

            // 文字入力モードを設定
            context.SetState(charInputStateInvoker);
        }

        /// <summary>
        /// Key入力時の数値（0〜9）判定
        /// </summary>
        /// <param name="consolekey"></param>
        /// <param name="iNumericKeyNo"></param>
        /// <returns>true:数値、false:数値以外</returns>
        private static bool IsNumericKey(ConsoleKey consolekey, ref int iNumericKeyNo)
        {
            switch (consolekey)
            {
                case ConsoleKey.D0:
                    iNumericKeyNo = 0;
                    return true;
                case ConsoleKey.D1:
                    iNumericKeyNo = 1;
                    return true;
                case ConsoleKey.D2:
                    iNumericKeyNo = 2;
                    return true;
                case ConsoleKey.D3:
                    iNumericKeyNo = 3;
                    return true;
                case ConsoleKey.D4:
                    iNumericKeyNo = 4;
                    return true;
                case ConsoleKey.D5:
                    iNumericKeyNo = 5;
                    return true;
                case ConsoleKey.D6:
                    iNumericKeyNo = 6;
                    return true;
                case ConsoleKey.D7:
                    iNumericKeyNo = 7;
                    return true;
                case ConsoleKey.D8:
                    iNumericKeyNo = 8;
                    return true;
                case ConsoleKey.D9:
                    iNumericKeyNo = 9;
                    return true;
            }
            return false;
        }
    }
}
