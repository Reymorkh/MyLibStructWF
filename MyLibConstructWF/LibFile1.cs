using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace MyLibStructWF
{
  public struct ArrayManagementWF
  {
    const double fromTop = 30, fromLeft = 60;
    static Size size = new Size(40, 20);
    public static List<TextBox> textBoxes = new List<TextBox>();
    public static List<Label> labels = new List<Label>();

    static int Partition(int[] array, int minIndex, int maxIndex)
    {
      var pen = minIndex - 1;
      for (var i = minIndex; i < maxIndex; i++)
      {
        if (array[i] > array[maxIndex])
        {
          pen++;
          (array[pen], array[i]) = (array[i], array[pen]);
        }
      }

      pen++;
      (array[pen], array[maxIndex]) = (array[maxIndex], array[pen]);
      return pen;
    }

    public static int[] HoarahSort(int[] array, int indexLeft, int indexRight)
    {
      if (indexLeft >= indexRight)
      {
        return array; //финальная сдача массива
      }

      var pivotIndex = Partition(array, indexLeft, indexRight); // определение опорного элемента в массиве и перестановка
      HoarahSort(array, indexLeft, pivotIndex - 1); // сорт по краям от опорного элемента
      HoarahSort(array, pivotIndex + 1, indexRight); // сорт по краям от опорного элемента

      return array;
    }

    public static void FormInit(Form form)
    {
      form.ShowDialog();
      form.Dispose();
      textBoxes.Clear();
      labels.Clear();
    }

    public static void AddBox(double posx, double posy)
    {
      TextBox newTextBox = new TextBox();
      newTextBox.Location = new Point(40 + Convert.ToInt32(Math.Round(fromLeft * posx, 0)), 60 + Convert.ToInt32(Math.Round(fromTop * posy, 0)));
      newTextBox.Size = size;
      newTextBox.MaxLength = 5;
      newTextBox.TextAlign = HorizontalAlignment.Center;
      textBoxes.Add(newTextBox);
    }

    public static void AddLabel(double posx, double posy, int number)
    {
      Label newLabel = new Label();
      newLabel.Location = new Point(40 + Convert.ToInt32(Math.Round(fromLeft * posx, 0)), 60 + Convert.ToInt32(Math.Round(fromTop * posy, 0)));
      newLabel.Text = Convert.ToString(number);
      newLabel.TextAlign = ContentAlignment.MiddleCenter;
      newLabel.AutoSize = true;
      labels.Add(newLabel);
    }

    public class OneDim
    {
      public int[] array = new int[0];

      public void Show(bool check, TextBox textBox)
      {
        if (check)
        {
          string[] text = new string[array.Length / 10 + 2];
          text[0] = "Одномерный массив:";
          int textIndex = 1;
          for (int temp = 0; temp < array.Length;temp +=10)
          {
            text[textIndex] = LineConverter(temp, text[textIndex]);
            textIndex++;
          }
          textBox.Text = string.Join(Environment.NewLine, text);
        }
        else
          MessageBox.Show("Массив пока не инициализирован.", "Ошибка");
      }

      private string LineConverter(int temp, string line)
      {
        for (int i = temp; i < temp + 10 && i < array.Length; i++)
          line += Convert.ToString(array[i]) + ' ';
        line = line.Trim();
        return line;
      }

      public void PrintBoxes()
      {
        AddLabel(-0.5, 0, 0 + 1);
        for (int i = 0; i < array.Length; i++)
        {
          AddBox(i, 0);
          AddLabel(i, -0.8, i + 1);
        }
        NumbersToBoxes();
      }

      public void NumbersToBoxes()
      {
        int boxIndex = 0;
        for (int i = 0; i < textBoxes.Count && i < array.Length; i++)
        {
          if (array[i] != 0)
            textBoxes[boxIndex].Text = Convert.ToString(array[i]);
          boxIndex++;

        }
      }

      public void BoxesToArray()
      {
        int temp;
        for (int i = 0; i < array.Length; i++)
        {
          if (textBoxes[i] != null && int.TryParse(textBoxes[i].Text, out temp))
            array[i] = Convert.ToInt32(textBoxes[i].Text);
        }
      }

      public void Task1(ref bool check)
      {
        if (check)
        {
          int i;
          for (i = 0; i < array.Length; i++)
            if (array[i] != 0 && array[i] % 2 == 0)
            {
              array[i] = 0;
              for (i += 1; i < array.Length; i++)
                (array[i - 1], array[i]) = (array[i], array[i - 1]);
              if (array.Length == 1)
                check = false;
              Array.Resize(ref array, array.Length - 1);
              break;
            }
          if (i == array.Length)
            MessageBox.Show("Чётных чисел не осталось.", "Предупрежедение");
        }
        else
          MessageBox.Show("Массива нет", "Предупреждение");
      }

      public int[] Copy() // для случаев, когда надо сделать не перменную-ссылку на другую переменную, а отдельный новый идентичный массив
      {
        return array;
      }
      
      public int Length()
      {
        return array.Length;
      }

      public void Save()
      {
        string path = "One Dimensional Array.txt";
        FileInit(path);
        StreamWriter file = new StreamWriter(path);
        for (int i = 0; i < array.Length; i++)
        {
          if (i != array.Length - 1)
            file.Write(array[i] + " ");
          else
            file.Write(array[i]);
        }
        file.Close();
        MessageBox.Show("Массив записан");
      }

      public bool IsFileCorrect(string text)
      {
        string[] arrStrings = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        if (arrStrings.Length == 1)
          return true;
        return false;
      }

      public int Load(string fileContent)
      {
        int errorNumber = 0;
        string[] contentLines = fileContent.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int[] arrayLocal = new int[contentLines.Length];
        for (int i = 0; i < contentLines.Length; i++)
        {
          int x;
          if (int.TryParse(contentLines[i], out x))
            arrayLocal[i] = x;
          else
            errorNumber++;
        }
        array = arrayLocal;
        return errorNumber;
      }

    }

    public class TwoDim
    {
      public int[,] array = new int[0,0];

      public void PrintBoxes()
      {
        for (int i = 0; i < array.GetLength(0); i++)
        {
          AddLabel(-0.5, i, i + 1); //принтит номера рядов
          for (int j = 0; j < array.GetLength(1); j++)
            AddBox(j, i);
        }
        for (int i = 0; i < array.GetLength(1); i++) //принтит номера столбцов
          AddLabel(i, -0.8, i + 1);
        NumbersToBoxes();
      }

      public void NumbersToBoxes()
      {
        int j, boxIndex = 0;
        for (int i = 0; i < array.GetLength(0); i++)
        {
          for (j = 0; j < array.GetLength(1); j++)
          {
            if (array[i, j] != 0)
              textBoxes[boxIndex].Text = Convert.ToString(array[i, j]);
            boxIndex++;
            if (boxIndex == textBoxes.Count)
              break;
          }
        }
      }

      public void BoxesToArray()
      {
        int j, boxIndex = 0, temp;
        for (int i = 0; i < array.GetLength(0); i++)
        {
          for (j = 0; j < array.GetLength(1); j++)
          {
            if (int.TryParse(textBoxes[boxIndex].Text, out temp))
              array[i, j] = temp;
            else
              array[i, j] = 0;
            boxIndex++;
          }
        }
      }

      public void Show(bool check, TextBox textBox)
      {
        if (check)
        {
          string[] text = new string[array.GetLength(0) + 1];
          text[0] = "Двумерный массив:";
          for (int i = 0; i < array.GetLength(0); i++)
          {
            for (int j = 0; j < array.GetLength(1); j++)
              text[i + 1] += Convert.ToString(array[i, j]) + " ";
            text[i + 1] = text[i + 1].Trim();
          }
          textBox.Text = string.Join(Environment.NewLine, text);
        }
        else
          MessageBox.Show("Массив пока не инициализирован.", "Ошибка");
      }

      public int[,] Copy()
      {
        return array;
      }

      public int Length(int temp)
      {
        switch (temp)
        {
          case 0:
            return array.GetLength(0);
            break;
          case 1: 
            return array.GetLength(1);
            break;
        }
        return -1;
      }

      public void Task2(int lineNumber)
      {
        lineNumber -= 1;
        int index1 = array.GetLength(0), index2 = array.GetLength(1), z = 0;
        int[,] temp = new int[index1 + 1, index2];
        for (int i = 0; i < index1; i++) //i - строки, j - содержимое строк
        {
          if (z == lineNumber)
            z++;
          for (int j = 0; j < index2; j++)
          {
            temp[z, j] = array[i, j];
          }
          if (z == lineNumber)
            z++;
          z++;
        }
        array = temp;
      }

      public void Save()
      {
        string path = "Two Dimensional Array.txt";
        FileInit(path);
        StreamWriter file = new StreamWriter(path);
        for (int i = 0; i < array.GetLength(0); i++)
        {
          for (int j = 0; j < array.GetLength(1); j++)
            if (j != array.GetLength(1) - 1)
              file.Write(array[i, j] + " ");
            else
              file.WriteLine(array[i, j]);
        }
        file.Close();
        MessageBox.Show("Массив записан");
      }

      public bool IsFileCorrect(string text)
      {
        string[] arrStrings = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int length = 0, lastLength = 0;
        foreach (string s in arrStrings)
        {
          string[] temp = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          length = temp.Length;
          if (Array.IndexOf(arrStrings, s) == 0) //Чтобы в самом начале приравнять последнюю длину к длине
            lastLength = length;
          if (length != lastLength)
            return false;
        }
        if (length == lastLength)
          return true;
        return false;
      }

      public int Load(string fileContent)
      {
        int errorNumber = 0;
        string[] contentLines = fileContent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] contentColumns = contentLines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int[,] arrayLocal = new int[contentLines.Length, contentColumns.Length];

        for (int i = 0; i < contentLines.Length; i++)
        {
          contentColumns = contentLines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          for (int j = 0; j < contentColumns.Length; j++)
          {
            int x;
            if (int.TryParse(contentColumns[j], out x))
              arrayLocal[i, j] = x;
            else
              errorNumber++;
          }
        }
        array = arrayLocal;
        return errorNumber;
      }
    }

    public class Torn
    {
      public int[][] array = new int[0][];

      public void PrintBoxes()
      {
        int length = 0;
        for (int i = 0; i < array.Length; i++)
        {
          AddLabel(-0.5, i, i + 1);
          if (length < array[i].Length)
            length = array[i].Length;
          for (int j = 0; j < array[i].Length; j++)
            AddBox(j, i);
        }
        for (int i = 0; i < length; i++)
          AddLabel(i, -0.8, i + 1);
        NumbersToBoxes();
      }

      public void NumbersToBoxes()
      {
        int boxIndex = 0;
        for (int i = 0; i < array.Length; i++)
        {
          for (int j = 0; j < array[i].Length; j++)
          {
            if (array[i][j] != 0)
              textBoxes[boxIndex].Text = Convert.ToString(array[i][j]);
            boxIndex++;
          }
        }
      }

      public void BoxesToArray()
      {
        int boxIndex = 0, temp;
        for (int i = 0; i < array.Length; i++)
        {
          for (int j = 0; j < array[i].Length; j++)
          {
            if (int.TryParse(textBoxes[boxIndex].Text, out temp))
              array[i][j] = temp;
            else
              array[i][j] = 0;
            boxIndex++;
          }
        }
      }

      public void Show(bool check, TextBox textBox)
      {
        string[] text = new string[array.Length + 1];
        text[0] = "Рваный массив:";
        if (check)
        {
          for (int i = 0; i < array.Length; i++)
          {
            for (int j = 0; j < array[i].Length; j++)
              text[i + 1] += array[i][j] + " ";
          }
          textBox.Text = string.Join(Environment.NewLine, text);
        }
        else
          MessageBox.Show("Массив не инициализирован.", "Ошибка");
      }

      public void Task3()
      {
        int maxLength = 0, maxLengthIndex = 0;
        for (int i = 0; i < array.Length; i++)  // индекс самой длинной строки массива
        {
          if (maxLength < array[i].Length)
          {
            maxLength = array[i].Length;
            maxLengthIndex = i;
          }
        }
        int[][] arrayTemp = new int[array.Length - 1][];
        int k = 0;
        for (int i = 0; i < array.Length; i++)
        {
          if (i != maxLengthIndex)
            arrayTemp[k++] = array[i];
        }
        array = arrayTemp;
      }

      public void Save()
      {
        string path = "Torn Array.txt";
        FileInit(path);
        StreamWriter file = new StreamWriter(path);
        for (int i = 0; i < array.Length; i++)
        {
          for (int j = 0; j < array[i].Length; j++)
            if (j != array[i].Length - 1)
              file.Write(array[i][j] + " ");
            else
              file.WriteLine(array[i][j]);
        }
        file.Close();
        MessageBox.Show("Массив записан");
      }

      public int[][] Copy()
      {
        return array;
      }

      public bool OldArrayCheck(int[][] arrayLocal)
      {
        if (arrayLocal.Length >= array.Length)
        {
          for (int i = 0; i < array.Length; i++)
            if (array[i].Length > arrayLocal[i].Length)
              return false;
        }
        else
          return false;
        return true;
      }

      public int Length()
      {
        return array.Length;
      }

      public bool IsFileCorrect(string text)
      {
        string[] arrStrings = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int length, lastLength = 0;
        foreach (string s in arrStrings)
        {
          string[] temp = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
          length = temp.Length;
          if (Array.IndexOf(arrStrings, s) == 0) //Чтобы в самом начале приравнять последнюю длину к длине
            lastLength = length;
          if (length != lastLength)
            return true;
        }
        return false;
      }

      public int Load(string fileContent)
      {
        int errorNumber = 0;
        string[] lineNumber = fileContent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] columnNubmer;
        int[][] arrayLocal = new int[lineNumber.Length][];
        for (int i = 0; i < lineNumber.Length; i++)
        {
          columnNubmer = lineNumber[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

          arrayLocal[i] = new int[columnNubmer.Length];
          for (int j = 0; j < arrayLocal[i].Length; j++)
          {
            int x;
            if (int.TryParse(columnNubmer[j], out x))
              arrayLocal[i][j] = x;
            else
              errorNumber++;
          }
        }
        array = arrayLocal;
        return errorNumber;
      }
    }

    public static string FileReader()
    {
      var fileContent = string.Empty;
      var filePath = string.Empty;

      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.InitialDirectory = "E:\\stuff\\Проекты VS\\Лабораторная работа №5\\Лабораторная работа №5\\bin\\Debug\\net7.0-windows";
        openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        openFileDialog.FilterIndex = 2;
        openFileDialog.RestoreDirectory = true;

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
          filePath = openFileDialog.FileName;
          var fileStream = openFileDialog.OpenFile();
          using (StreamReader reader = new StreamReader(fileStream))
          {
            fileContent = reader.ReadToEnd();
          }
        }
      }
      return fileContent;
    }

    static void FileInit(string path)
    {
      if (!File.Exists(path))
      {
        var fie = File.Create(path);
        fie.Close();
      }
      else
        File.WriteAllText(path, string.Empty);
    }
  }
}