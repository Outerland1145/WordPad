using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WordPad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            InitializeFontComboBox();
            InitializeFontSizeComboBox();
            InitializeFontStyleComboBox();
        }
        bool isUndoRedo = false;
        bool isUndo = false;
        bool isRedo = false;
        String storage_cache;
        private Stack<string> textHistory = new Stack<string>();
        private Stack<string> UndoHistory = new Stack<string>();
        private const int MaxRecoverHistoryCount = 10; // 最多紀錄10個紀錄
        private const int MaxHistoryCount = 10; // 最多紀錄10個紀錄
        private int selectionStart = 0;
        private int selectionLength = 0;
        private void InitializeFontComboBox()
        {
            foreach (FontFamily font in FontFamily.Families)
            {
                Font.Items.Add(font.Name);
            }
            Font.SelectedIndex = 0;
        }

        // 字體大小初始化
        private void InitializeFontSizeComboBox()
        {
            for (int i = 8; i <= 72; i += 2)
            {
                FontSize.Items.Add(i);
            }
            FontSize.SelectedIndex = 2;
        }

        // 字體樣式初始化
        private void InitializeFontStyleComboBox()
        {
            FontStyleCombox.Items.Add(FontStyle.Regular.ToString());
            FontStyleCombox.Items.Add(FontStyle.Bold.ToString());
            FontStyleCombox.Items.Add(FontStyle.Italic.ToString());
            FontStyleCombox.Items.Add(FontStyle.Underline.ToString());
            FontStyleCombox.Items.Add(FontStyle.Strikeout.ToString());
            FontStyleCombox.SelectedIndex = 0;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionStart = RTBTextBox.SelectionStart;
            selectionLength = RTBTextBox.SelectionLength;
            if (RTBTextBox.SelectionFont != null)
            {
                // 確保選擇的字型、大小和樣式都不為 null
                string selectedFont = Font.SelectedItem?.ToString();
                string selectedSizeStr = FontSize.SelectedItem?.ToString();
                string selectedStyleStr = FontStyleCombox.SelectedItem?.ToString();

                if (selectedFont != null && selectedSizeStr != null && selectedStyleStr != null)
                {
                    float selectedSize = float.Parse(selectedSizeStr);
                    FontStyle selectedStyle = (FontStyle)Enum.Parse(typeof(FontStyle), selectedStyleStr);

                    // 獲取當前選擇的文字的字型和樣式
                    Font currentFont = RTBTextBox.SelectionFont;
                    FontStyle newStyle = currentFont.Style;

                    // 檢查是否需要應用新的樣式
                    if (FontStyleCombox.SelectedItem.ToString() == FontStyle.Bold.ToString())
                        newStyle = FontStyle.Bold;
                    else if (FontStyleCombox.SelectedItem.ToString() == FontStyle.Italic.ToString())
                        newStyle = FontStyle.Italic;
                    else if (FontStyleCombox.SelectedItem.ToString() == FontStyle.Underline.ToString())
                        newStyle = FontStyle.Underline;
                    else if (FontStyleCombox.SelectedItem.ToString() == FontStyle.Strikeout.ToString())
                        newStyle = FontStyle.Strikeout;
                    else
                        newStyle = FontStyle.Regular;

                    Font newFont = new Font(selectedFont, selectedSize, newStyle);
                    RTBTextBox.SelectionFont = newFont;
                }
            }
            RTBTextBox.Focus();
            RTBTextBox.Select(selectionStart, selectionLength);
        }
        

        private void Save_Click(object sender, EventArgs e)
        {
            // 設置對話方塊標題
            saveFileDialog1.Title = "儲存檔案";
            // 設置對話方塊篩選器，限制使用者只能選擇特定類型的檔案
            saveFileDialog1.Filter = "文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
            // 如果希望預設開啟的檔案類型是文字檔案，可以這樣設置
            saveFileDialog1.FilterIndex = 1;
            // 如果希望對話方塊在開啟時顯示的初始目錄，可以設置 InitialDirectory
            saveFileDialog1.InitialDirectory = "C:\\";

            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string saveFileName = saveFileDialog1.FileName;
                    using (StreamWriter writer = new StreamWriter(saveFileName))
                    {
                        // 要寫入的資料
                        string data = RTBTextBox.Text;

                        // 將資料寫入檔案
                        writer.WriteLine(data);
                    }
                }
                catch (Exception ex)
                {
                    // 如果發生錯誤，用MessageBox顯示錯誤訊息
                    MessageBox.Show("讀取檔案時發生錯誤: " + ex.Message, "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("使用者取消了儲存檔案操作。", "訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
        }
        private void Load_Click(object sender, EventArgs e)
        {
            // 設置對話方塊標題
            openFileDialog1.Title = "選擇檔案";
            // 設置對話方塊篩選器，限制使用者只能選擇特定類型的檔案
            openFileDialog1.Filter = "文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
            // 如果希望預設開啟的檔案類型是文字檔案，可以這樣設置
            openFileDialog1.FilterIndex = 1;
            // 如果希望對話方塊在開啟時顯示的初始目錄，可以設置 InitialDirectory
            openFileDialog1.InitialDirectory = "C:\\";
            // 允許使用者選擇多個檔案
            openFileDialog1.Multiselect = true;

            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                try
                {
                    // 使用者在OpenFileDialog選擇的檔案
                    string selectedFileName = openFileDialog1.FileName;

                    //// 使用 FileStream 打開檔案
                    //// 建立一個檔案資料流，並且設定檔案名稱與檔案開啟模式為「開啟檔案」
                    //FileStream fileStream = new FileStream(selectedFileName, FileMode.Open, FileAccess.Read);
                    //// 讀取資料流
                    //StreamReader streamReader = new StreamReader(fileStream);
                    //// 將檔案內容顯示到 RichTextBox 中
                    //rtbText.Text = streamReader.ReadToEnd();
                    //// 關閉資料流與讀取資料流
                    //fileStream.Close();
                    //streamReader.Close();

                    // 使用 using 與 FileStream 打開檔案
                    using (FileStream fileStream = new FileStream(selectedFileName, FileMode.Open, FileAccess.Read))
                    {
                        // 使用 StreamReader 讀取檔案內容
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                        {
                            // 將檔案內容顯示到 RichTextBox 中
                            RTBTextBox.Text = streamReader.ReadToEnd();
                        }
                    }

                    //// 更為簡單的做法，將檔案內容顯示到 RichTextBox 中
                    //string fileContent = File.ReadAllText(selectedFileName);
                    //rtbText.Text = fileContent;
                }
                catch (Exception ex)
                {
                    // 如果發生錯誤，用MessageBox顯示錯誤訊息
                    MessageBox.Show("讀取檔案時發生錯誤: " + ex.Message, "錯誤訊息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("使用者取消了選擇檔案操作。", "訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
        }

        private void Undo_Click(object sender, EventArgs e)
        {
            isUndoRedo = true;
            if (textHistory.Count > 1)
            {
                textHistory.Pop(); // 移除當前的文本內容
                storage_cache = RTBTextBox.Text;
                UndoHistory.Push(RTBTextBox.Text); // 將原文本內容塞入回復堆疊中
                RTBTextBox.Text = textHistory.Peek(); // 將堆疊頂部的文本內容設置為當前的文本內容
                if (UndoHistory.Count > MaxRecoverHistoryCount)
                {
                    // 移除最底下的一筆資料
                    Stack<string> tempStack = new Stack<string>();
                    for (int i = 0; i < MaxRecoverHistoryCount; i++)
                    {
                        tempStack.Push(UndoHistory.Pop());
                    }
                    UndoHistory.Pop(); // 移除最底下的一筆資料
                    foreach (string item in tempStack)
                    {
                        UndoHistory.Push(item);
                    }
                }
            }
            UpdateRedoBox(); // 更新 RedoBox
            UpdateListBox(); // 更新 ListBox

            isUndoRedo = false;
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if (UndoHistory.Count > 1)
            {
                isUndoRedo = true;
                UndoHistory.Push(UndoHistory.Pop()); // 將回復堆疊最上面的紀錄移出，再堆到重作堆疊
                RTBTextBox.Text = UndoHistory.Peek(); // 將回復堆疊最上面一筆紀錄顯示
                UpdateRedoBox();
                isUndoRedo = false;
            }
            //if (UndoHistory.Count > 1)
            //{
               // UndoHistory.Pop(); // 移除當前的文本內容
               // RTBTextBox.Text = UndoHistory.Peek(); // 將堆疊頂部的文本內容設置為當前的文本內容
               // storage_cache = UndoHistory.Peek();
            //}
           // else
            //{
                //RTBTextBox.Text = storage_cache;
               //Redobox.Items.Clear();
            //}
            //UpdateRedoBox();
            //isUndoRedo = false;
        }
        void UpdateRedoBox()
        {
            Redobox.Items.Clear(); // 清空 ListBox 中的元素

            // 將堆疊中的內容逐一添加到 ListBox 中
            foreach (string item in UndoHistory)
            {
                Redobox.Items.Add(item);
            }
        }
        void UpdateListBox()
        {
            Listbox.Items.Clear(); // 清空 ListBox 中的元素

            // 將堆疊中的內容逐一添加到 ListBox 中
            foreach (string item in textHistory)
            {
                Listbox.Items.Add(item);
            }
        }

        private void RTBTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!isUndoRedo)
            {
                // 將當前的文本內容加入堆疊
                textHistory.Push(RTBTextBox.Text);

                // 確保堆疊中只保留最多10個紀錄
                if (textHistory.Count > MaxHistoryCount)
                {
                    // 移除最底下的一筆資料
                    Stack<string> tempStack = new Stack<string>();
                    for (int i = 0; i < MaxHistoryCount; i++)
                    {
                        tempStack.Push(textHistory.Pop());
                    }
                    textHistory.Pop(); // 移除最底下的一筆資料
                    foreach (string item in tempStack)
                    {
                        textHistory.Push(item);
                    }
                }
                UpdateListBox(); // 更新 ListBox
            }
        }

        private void Clear_MouseClick(object sender, MouseEventArgs e)
        {
            Redobox.Items.Clear();
            Listbox.Items.Clear();
            textHistory.Clear();
            UndoHistory.Clear();


        }
    }
}
