using System;
using System.IO;
using System.Text;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-solid & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    //Implement IDisposable to ensure resources are correctly disposed. 
    public class CSVReaderWriter : IDisposable
    {
        private StreamReader _readerStream = null;
        private StreamWriter _writerStream = null;

        private bool m_disposed = false;

        [Flags]
        public enum Mode { Read = 1, Write = 2 };

        public bool Open(string fileName, Mode mode)
        {
            try {
                //Make use of switch instead of previous use of if statements
                switch (mode) {
                    case Mode.Read:
                        //Updated to use StreamReader instead for File.Opentext for better control of buffer, however 
                        //File.ReadAllLines(fileName) may provide the best performance but at cost of memory consumption
                        _readerStream = new StreamReader(fileName);
                        return true;
                    case Mode.Write:
                        _writerStream = new StreamWriter(fileName, true); 
                        return true;
                    default:
                        //Unknown request 
                        return false;
                }
            }
            //Implemented try catch for catching exceptions, not in else condition. The exception 
            //could be further updated to use string interpolaton is using latest version of C# compiler
            catch (ApplicationException ex) {
                throw new Exception("Unknown file mode for " + fileName + " " + ex.Message);
            }
        }

        public void Write(params string[] columns)
        {
            int i = 0;
            //Use StringBuilder instead of String
            StringBuilder sb = new StringBuilder();

            //Use foreach instead of for. Foreach could be considered faster and better from a refactoring 
            //point of view
            foreach (string column in columns) {
                sb.Append(column);
                if (i++ < columns.Length - 1) {
                    sb.Append('\t');
                }
            }
            _writerStream.WriteLine(sb);
        }

        //Only one Read method is required 
        public bool Read(out string column1, out string column2)
        {
            const int FIRST_COLUMN = 0;
            const int SECOND_COLUMN = 1;

            string line = _readerStream.ReadLine();

            //Removed the if conditions to check null and zero length and replaced with native string implementation
            if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) {
                column1 = column2 = null;
                return false;
            }
            else {
                string[] columns = line.Split('\t');
                column1 = columns[FIRST_COLUMN];
                column2 = columns[SECOND_COLUMN];
                return true;
            }
        }

        //Removed Close() in place of IDisposable and Dispose() implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed) {
                if (disposing) {
                    if (_writerStream != null) {
                        _writerStream.Close();
                        _writerStream.Dispose();
                    }

                    if (_readerStream != null) {
                        _readerStream.Close();
                        _readerStream.Dispose();
                    }
                  
                }
                m_disposed = true;
            }
        }
    }
}
