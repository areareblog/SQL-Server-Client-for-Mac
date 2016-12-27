using System;
using System.Collections.Generic;

namespace SQLClient.Mac
{
	public class MainTreeNode : Foundation.NSObject
	{
		public MainTreeNodeType NodeType
		{
			get;
			set;
		}

		public string Data
		{
			get;
			set;
		}

		public object Information
		{
			get;
			set;
		}
			

		public List<MainTreeNode> Children
		{
			get;
			private set;
		}

		public MainTreeNode(string data)
		{
			this.Data = data;
			this.Children = new List<MainTreeNode>();
		}

		public bool IsLeaf
		{
			get { return ChildCount == 0; }
		}

		public MainTreeNode GetChild(int index)
		{
			return Children[index];
		}

		public int ChildCount { get { return Children.Count; } }
		
	}

	public enum MainTreeNodeType
	{
		Normal,
		Folder,
		Database,
		Table
	}
}
