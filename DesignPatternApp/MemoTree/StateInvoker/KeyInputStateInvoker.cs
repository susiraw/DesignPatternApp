using System;

namespace MemoTree
{
    /// <summary>
    /// キー入力モードのInvoker
    /// </summary>
    internal class KeyInputStateInvoker : StateInvoker
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
        internal static StateInvoker GetInstance()
        {
            if (stateInvoker == null)
            {
                stateInvoker = new KeyInputStateInvoker();
            }
            return stateInvoker;
        }
        #endregion

        /// <summary>
        /// キー入力モードの処理を行う
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true:処理の続行、false:処理の終了</returns>
        internal override bool Execute(Context context)
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
                            // Undo処理
                            if (context.m_undoStack.Count > 0)
                            {
                                var undoStackCommand = context.m_undoStack.Pop();
                                undoStackCommand.m_undoCommand.CallExecute(context, null, false);
                                context.m_redoStack.Push(undoStackCommand);
                            }
                            return true;
                        case ConsoleKey.Y:
                            // Redo処理
                            if (context.m_redoStack.Count > 0)
                            {
                                var redoStackCommand = context.m_redoStack.Pop();
                                redoStackCommand.CallExecute(context, null);
                            }
                            return true;
                    }
                }
                else if (IsNumericKey(keyInfo.Key, ref iNumericKeyNo))
                {
                    // 数値（0〜9） + Key
                    if (context.m_currentComponent.m_listComponent.Count <= iNumericKeyNo)
                    {
                        Console.WriteLine("No." + iNumericKeyNo + " is nothing.");
                    }
                    else
                    {
                        keyInfo = Console.ReadKey(true);
                        switch (keyInfo.Key)
                        {
                            case ConsoleKey.Enter:
                                // Enter処理
                                this.ExecuteEnterCommand(context, iNumericKeyNo);
                                return true;
                            case ConsoleKey.D:
                                // delete処理
                                Console.WriteLine(" is nothing.");
                                this.ExecuteDeleteCommand(context, iNumericKeyNo);
                                return true;
                            case ConsoleKey.E:
                                // Edit処理
                                this.SetCharInputStateInvoker(context, false, iNumericKeyNo);
                                return true;
                            default:
                                Console.WriteLine(keyInfo.Key + " is nothing.");
                                break;
                        }   
                    }
                }
                else
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Z:
                            keyInfo = Console.ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                // Enter処理（戻る）
                                if (this.ExecuteEnterCommand(context))
                                {
                                    return true;
                                }
                            }
                            break;
                        case ConsoleKey.Escape:
                            return false;
                        default:
                            Console.WriteLine("対応Keyなし：" + keyInfo.Key);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Enterコマンドを実行する。
        /// </summary>
        /// <param name="context">Context.</param>
        private bool ExecuteEnterCommand(Context context, int iNumericKeyNo = -1)
        {
            // Enterコマンドを生成し、実行
            var enterCommand = new EnterCommand();
            Component component = null;
            if (iNumericKeyNo == -1)
            {
                // 親コンポーネントを取得
                var parentComponent = this.GetParentComponent(context.m_componentMain, context.m_currentComponent);
                if (parentComponent == null)
                {
                    return false;
                }
                component = parentComponent;
            }
            else
            {
                component = context.m_currentComponent.m_listComponent[iNumericKeyNo];
            }
            enterCommand.CallExecute(context, component);
            return true;
        }

        /// <summary>
        /// 親コンポーネントを取得
        /// 全コンポーネントから対象コンポーネントの親コンポーネントを再帰的に取得する。
        /// ※MemoTree直下の場合、nullが返却される。
        /// </summary>
        /// <param name="parentComponent"></param>
        /// <param name="childComponent"></param>
        /// <returns>親コンポーネント</returns>
        private Component GetParentComponent(DirComponent parentComponent, DirComponent childComponent)
        {
            foreach (var component in parentComponent.m_listComponent)
            {
                if (component == childComponent)
                {
                    return parentComponent;
                }
                if (component.GetType() == typeof(DirComponent))
                {
                    var recursiveComponent = GetParentComponent((DirComponent)component, childComponent);
                    if (recursiveComponent != null)
                    {
                        return recursiveComponent;  
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// deleteコマンドを実行する。
        /// </summary>
        /// <param name="context">Context.</param>
        private void ExecuteDeleteCommand(Context context, int iNumericKeyNo)
        {
            // Deleteコマンドを生成し、実行
            var deleteCommand = new DeleteCommand();
            deleteCommand.CallExecute(context, context.m_currentComponent.m_listComponent[iNumericKeyNo]);
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
                // 上限チェック
                if (context.m_currentComponent.m_listComponent.Count > Define.MAX_COMPONENT_NUM)
                {
                    Console.WriteLine("Can't Create any more.");
                    return;
                }
                Console.WriteLine("D Create directory, F Create file");

                var bContinue = true;
                var comopnentType = Define.ComponentType.Dir;
                while (bContinue)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D:
                            Console.WriteLine("Please input directory name.");
                            comopnentType = Define.ComponentType.Dir;
                            bContinue = false;
                            break;
                        case ConsoleKey.F:
                            Console.WriteLine("Please input file name.");
                            comopnentType = Define.ComponentType.File;
                            bContinue = false;
                            break;
                        default:
                            Console.WriteLine("Please press D or F key.");
                            break;
                    }
                }

                // Createコマンドを生成し、KeyStateInvoker、CharStateInvokerに設定
                var createCommand = new CreateCommand();
                createCommand.m_componentType = comopnentType;
                createCommand.SetComponent(context.m_currentComponent);
                context.m_stateInvoker.SetCommand(createCommand);
                charInputStateInvoker.SetCommand(createCommand);
            }
            else
            {
                // Editコマンドを生成し、KeyStateInvoker、CharStateInvokerに設定
                Console.WriteLine("Please input name.");
                var editCommand = new EditCommand();
                editCommand.SetComponent(context.m_currentComponent.m_listComponent[iNumericKeyNo]);
                context.m_stateInvoker.SetCommand(editCommand);
                charInputStateInvoker.SetCommand(editCommand);
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
