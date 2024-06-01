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
        String storage_cache;
        private Stack<MemoryStream> undoStack = new Stack<MemoryStream>(); // 回復堆疊
        private Stack<MemoryStream> redoStack = new Stack<MemoryStream>(); // 重作堆疊
        //private Stack<string> textHistory = new Stack<string>();
        //private Stack<string> UndoHistory = new Stack<string>();
        private const int MaxRecoverHistoryCount = 10; // 最多紀錄10個紀錄
        private const int MaxHistoryCount = 10; // 最多紀錄10個紀錄
        private int selectionStart = 0;                            // 記錄文字反白的起點
        private int selectionLength = 0;                           // 記錄文字反白的長度
        private void InitializeFontComboBox()
        {
            foreach (FontFamily font in FontFamily.Families)
            {
                Font.Items.Add(font.Name);
            }
            Font.SelectedIndex = 0;
            Console.WriteLine("Font ComboBox initialized with fonts:");
            foreach (var item in Font.Items)
            {
                Console.WriteLine(item);
            }
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
                    if (float.TryParse(selectedSizeStr, out float selectedSize))
                    {
                        // 獲取當前選擇的文字的字型和樣式
                        Font currentFont = RTBTextBox.SelectionFont;
                        FontStyle newStyle = currentFont.Style;

                        // 更新樣式，允許多種樣式的組合
                        newStyle = FontStyle.Regular;
                        if (selectedStyleStr.Contains("Bold"))
                            newStyle |= FontStyle.Bold;
                        if (selectedStyleStr.Contains("Italic"))
                            newStyle |= FontStyle.Italic;
                        if (selectedStyleStr.Contains("Underline"))
                            newStyle |= FontStyle.Underline;
                        if (selectedStyleStr.Contains("Strikeout"))
                            newStyle |= FontStyle.Strikeout;

                        try
                        {
                            // 創建新字體
                            Font newFont = new Font(selectedFont, selectedSize, newStyle);
                            RTBTextBox.SelectionFont = newFont;
                            // 日誌訊息，確保字體已正確設置
                            Console.WriteLine($"Font changed to: {newFont.Name}, Size: {newFont.Size}, Style: {newFont.Style}");
                        }
                        catch (Exception ex)
                        {
                            // 處理無效的字型名稱或其他錯誤
                            MessageBox.Show($"無法應用字型: {selectedFont}\n錯誤訊息: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        // 處理無效的字型大小
                        MessageBox.Show("選擇的字型大小無效", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            saveFileDialog1.Filter = "RTF格式檔案 (*.rtf)|*.rtf|文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
            // 如果希望預設開啟的檔案類型是文字檔案，可以這樣設置
            saveFileDialog1.FilterIndex = 1;
            // 如果希望對話方塊在開啟時顯示的初始目錄，可以設置 InitialDirectory
            saveFileDialog1.InitialDirectory = "C:\\";

            DialogResult result = saveFileDialog1.ShowDialog();
            FileStream fileStream = null;
            if (result == DialogResult.OK)
            {
                try
                {
                    // 使用者指定的儲存檔案的路徑
                    string saveFileName = saveFileDialog1.FileName;
                    string extension = Path.GetExtension(saveFileName);

                    // 使用 using 與 FileStream 建立檔案，如果檔案已存在則覆寫
                    using (fileStream = new FileStream(saveFileName, FileMode.Create, FileAccess.Write))
                    {
                        if (extension.ToLower() == ".txt")
                        {
                            // 將 RichTextBox 中的文字寫入檔案中
                            byte[] data = Encoding.UTF8.GetBytes(RTBTextBox.Text);
                            fileStream.Write(data, 0, data.Length);
                        }
                        else if (extension.ToLower() == ".rtf")
                        {
                            // 將RichTextBox中的內容保存為RTF格式
                            RTBTextBox.SaveFile(fileStream, RichTextBoxStreamType.RichText);
                        }
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
            openFileDialog1.Filter = "RTF格式檔案 (*.rtf)|*.rtf|文字檔案 (*.txt)|*.txt|所有檔案 (*.*)|*.*";
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

                    // 獲取文件的副檔名
                    string fileExtension = Path.GetExtension(selectedFileName).ToLower();

                    // 判斷副檔名是甚麼格式
                    if (fileExtension == ".txt")
                    {
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
                    }
                    else if (fileExtension == ".rtf")
                    {
                        // 如果是RTF文件，使用RichTextBox的LoadFile方法
                        RTBTextBox.LoadFile(selectedFileName, RichTextBoxStreamType.RichText);
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
                MessageBox.Show("使用者取消了選擇檔案操作。", "訊息", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
        }
        private void SaveCurrentStateToStack()
        {
            // 創建一個新的 MemoryStream 來保存文字編輯狀態
            MemoryStream memoryStream = new MemoryStream();
            // 將 RichTextBox 的內容保存到 memoryStream
            RTBTextBox.SaveFile(memoryStream, RichTextBoxStreamType.RichText);
            // 將 memoryStream 放入回復堆疊
            undoStack.Push(memoryStream);
        }

        // 將文字狀態從記憶體中顯示到 RichTextBox
        private void LoadFromMemory(MemoryStream memoryStream)
        {
            // 將 memoryStream 的指標重置到開始位置
            memoryStream.Seek(0, SeekOrigin.Begin);
            // 將 memoryStream 的內容放到到 RichTextBox
            RTBTextBox.LoadFile(memoryStream, RichTextBoxStreamType.RichText);
        }
        private void Undo_Click(object sender, EventArgs e)
        {
            isUndoRedo = true;
            if (undoStack.Count > 1)
            {
                isUndoRedo = true;
                redoStack.Push(undoStack.Pop()); // 將回復堆疊最上面的紀錄移出，再堆到重作堆疊
                MemoryStream lastSavedState = undoStack.Peek(); // 將回復堆疊最上面一筆紀錄顯示
                LoadFromMemory(lastSavedState);
                isUndoRedo = false;
            }

            isUndoRedo = false;
        }

        private void Redo_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                isUndoRedo = true;
                undoStack.Push(redoStack.Pop()); // 將重作堆疊最上面的紀錄移出，再堆到回復堆疊
                MemoryStream lastSavedState = undoStack.Peek(); // 將回復堆疊最上面一筆紀錄顯示
                LoadFromMemory(lastSavedState);
                isUndoRedo = false;
            }
        }

        private void RTBTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!isUndoRedo)
            {
                // 將當前的文本內容加入堆疊
                SaveCurrentStateToStack(); // 將當前的文本內容加入堆疊
                redoStack.Clear();            // 清空重作堆疊
                // 確保堆疊中只保留最多10個紀錄
                if (undoStack.Count > MaxHistoryCount)
                {
                    // 用一個臨時堆疊，將除了最下面一筆的文字記錄之外，將文字紀錄堆疊由上而下，逐一移除再堆疊到臨時堆疊之中
                    Stack<MemoryStream> tempStack = new Stack<MemoryStream>();
                    for (int i = 0; i < MaxHistoryCount; i++)
                    {
                        tempStack.Push(undoStack.Pop());
                    }
                    undoStack.Clear(); // 清空堆疊
                                       // 文字編輯堆疊紀錄清空之後，再將暫存堆疊（tempStack）中的資料，逐一放回到文字編輯堆疊紀錄
                    foreach (MemoryStream item in tempStack)
                    {
                        undoStack.Push(item);
                    }
                }
            }
        }

        private void Clear_MouseClick(object sender, MouseEventArgs e)
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
