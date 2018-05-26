using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MemoTree
{
    /// <summary>
    /// ディレクトリコンポーネント
    /// </summary>
    public class DirComponent : Component
    {
        // TODO 親クラスで定義するか検討
        // サブコンポーネント
        protected List<Component> m_listComponent;

        /// <summary>
        /// コンストラクタ
        /// 各メンバーの設定を行う
        /// </summary>
        /// <param name="strPath"></param>
        public DirComponent(string strPath)
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
        public static void SetComponent(DirComponent dirComponent)
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
        /// コンポーネントの取得
        /// 対象となるDirComponent配下から対象パスのコンポーネントを再帰的に取得する。
        /// </summary>
        /// <param name="dirComponent"></param>
        /// <param name="strPath"></param>
		/// <returns>対象パスのコンポーネント</returns>
        public static Component GetComponent(DirComponent dirComponent, string strPath)
        {
            // TODO パス以外に取得方法がないか検討
            foreach (var component in dirComponent.m_listComponent)
            {
                if (component.m_strPath == strPath)
                {
                    return component;
                }
                if (component.GetType() == typeof(DirComponent))
                {
                    GetComponent((DirComponent)component, strPath);
                }
            }
            return null;
        }

        /// <summary>
        /// 対象となるDirComponent直下のコンポーネントの名前を全て取得する。
        /// </summary>
		/// <param name="dirComponent"></param>
        /// <returns>ファイル名のリスト</returns>
        public static List<string> GetAllFileName(DirComponent dirComponent)
        {
            return dirComponent.m_listComponent.Select(x => x.m_strName).ToList();
        }
    }
}
