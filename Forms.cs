
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// classes supporting application GUI
/// all Linux/Windows interoperability issues should be handled here
/// </summary>
namespace JQuantForms
{

	public interface IConsoleOut
	{
		void Write(string s);
	}

	public class ConsoleOutDummy : IConsoleOut
	{
		public void Write(string s)
		{
		}
	}

	/// <summary>
	/// Graphic console - exactly like text based console
	/// This console for output only
	/// </summary>
	public class ConsoleOut : TextBox, IConsoleOut
	{
		public ConsoleOut()
		{
			base.Multiline = true;
			base.ScrollBars = ScrollBars.Vertical;
			base.ReadOnly = true;
			base.WordWrap = true;
			base.Font = new System.Drawing.Font("Lucida Console", 10);
			setTextDelegate = new SetTextDelegate(SetText);
		}

		public void Write(string s)
		{
			if (this.InvokeRequired)
			{
				// It's on a different thread, so use Invoke.
				this.Invoke(setTextDelegate, new object[] { s });
			}
			else
			{
				// It's on the same thread, no need for Invoke
				SetText(s);
			}
		}

		private void SetText(string s)
		{
			const int maxTextLength = 32 * 1024;

			// prepare string (truncate if neccessary)

			if (TextLength > maxTextLength)
			{
				// truncate by at least 30% to avoid truncation on every string
				int removeCount = Math.Max(TextLength - maxTextLength, (int)(maxTextLength * 0.3F));

				Text.Remove(0, removeCount);

				SelectionStart = Text.Length;
			}

			base.AppendText(s);
			//            base.ScrollToCaret();
		}

		protected delegate void SetTextDelegate(string s);
		SetTextDelegate setTextDelegate;

	}

	/// <summary>
	/// Graphic console - exactly like text based console
	/// This console for input only
	/// </summary>
	public class ConsoleIn : TextBox
	{
		public delegate void ProcessCommandDelegate(string command);

		public ConsoleIn(ProcessCommandDelegate commandProcessor)
		{
			base.Multiline = false;
			base.ReadOnly = false;
			base.ScrollBars = ScrollBars.Horizontal;
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressHandler);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyHandler);

			this.commandProcessor = commandProcessor;
			this.history = new List<string>(HISTORY_SIZE);
			historyIdx = 0;
		}

		public string ReadLine()
		{
			return base.Text;
		}

		protected void KeyHandler(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			System.Windows.Forms.Keys keyCode = e.KeyCode;
			switch (keyCode)
			{
				case System.Windows.Forms.Keys.Up:
					if (historyIdx > 0)
					{
						historyIdx--;
					}
					if ((historyIdx >= 0) && (history.Count > 0))
					{
						Text = history[historyIdx];
					}
					break;
				case System.Windows.Forms.Keys.Down:
					if (historyIdx < (history.Count - 1))
					{
						historyIdx++;
					}
					if ((historyIdx < history.Count) && (history.Count > 0))
					{
						Text = history[historyIdx];
					}
					break;
			}
		}

		protected void KeyPressHandler(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			System.Windows.Forms.Keys keyPressed = (System.Windows.Forms.Keys)e.KeyChar;
			switch (keyPressed)
			{
				case System.Windows.Forms.Keys.Escape:
					this.Clear();
					break;
				case System.Windows.Forms.Keys.Enter:
					commandProcessor(Text);
					UpdateHistory(Text);
					break;
			}
		}

		protected void UpdateHistory(string s)
		{
			bool contains = history.Contains(s);
			if (!contains)
			{
				history.Add(s);
			}
			if (history.Count >= HISTORY_SIZE)
			{
				history.RemoveAt(0);
			}
		}

		protected ProcessCommandDelegate commandProcessor;
		protected List<string> history;
		protected int historyIdx;
		protected const int HISTORY_SIZE = 100;
	}

}//namespace JQuantForms
