using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MemoTree
{
    /// <summary>
    /// ディレクトリコンポーネント
    /// </summary>
    internal class DirComponent : Component
    {
        // サブコンポーネント
        internal List<Component> m_listComponent;

        /// <summary>
        /// コンストラクタ
        /// 各メンバーの設定を行う
        /// </summary>
        /// <param name="strPath"></param>
        internal DirComponent(string strPath)
        {
            this.m_strPath = strPath;
            this.m_strName = Path.GetFileName(strPath);
            this.m_listComponent = new List<Component>();
        }

        /// <summary>
        /// コンポーネントの設定
        /// 対象となるDirComponent配下のコンポーネントを再帰的に取得、設定する。
        /// </summary>
        /// <param name="dirComponent"></param>
        internal static void SetComponent(DirComponent dirComponent)
        {
            foreach (var dir in Directory.GetDirectories(dirComponent.m_strPath))
            {
                var dirComponentSub = new DirComponent(dir);
                SetComponent(dirComponentSub);
                dirComponent.m_listComponent.Add(dirComponentSub);
            }
            foreach (var file in Directory.GetFiles(dirComponent.m_strPath, "*.txt"))
            {
                var fileComponent = new FileComponent(file);
                dirComponent.m_listComponent.Add(fileComponent);
            }
        }

        /// <summary>
        /// 対象となるDirComponent直下のコンポーネントの名前を全て取得する。
        /// </summary>
		/// <param name="dirComponent"></param>
        /// <returns>ファイル名のリスト</returns>
        internal static List<string> GetAllFileName(DirComponent dirComponent)
        {
            return dirComponent.m_listComponent.Select(x => x.m_strName).ToList();
        }

        /// <summary>
        /// サブコンポーネントの追加を行う
        /// DirComponentでのみ使用するため、仮想メソッドにて定義
        /// </summary>
        internal override void Add(Component component)
        {
            this.m_listComponent.Add(component);
        }

        /// <summary>
        /// サブコンポーネントの削除を行う
        /// 対象となるDirComponent配下のコンポーネントを再帰的に取得し、ファイル名を出力後、削除する。
        /// </summary>
        internal override void Remove(Context context, ref List<string> listDeleted)
        {
            listDeleted.Add(base.m_strName);
            foreach(var component in this.m_listComponent)
            {
                component.Remove(context, ref listDeleted);
            }
            Directory.Delete(base.m_strPath);
            context.m_currentComponent.m_listComponent.Remove(this);
        }
    }
}
