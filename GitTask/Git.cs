using System;
using System.Collections.Generic;

namespace GitTask
{
    public class Git
    {
        private const int MaxNumberOfActions = 50000;

        private int FilesCount = 0;

        private Dictionary<int, List<int>> Files;

        private Dictionary<int, int> temporaryFiles;

        private int countOfCommit = 0;

        private int numberOfActions = 0;

        public Git(int filesCount)
        {
            if (filesCount > 50000 || filesCount<1)
            {
                throw new ArgumentOutOfRangeException("filesCount не попадает в интервал допустимых значений [1-50000]");
            }
            else
            {
                FilesCount = filesCount;
                Files = new Dictionary<int, List<int>>(FilesCount);
                temporaryFiles = new Dictionary<int, int>(FilesCount);
            }
        }

        public void Update(int fileNumber, int value)
        {
            /*if(numberOfActions>MaxNumberOfActions)
            {
                return;
            }*/
            if (fileNumber > FilesCount || fileNumber < 0)
            {
                throw new ArgumentOutOfRangeException("fileNumber не попадает в интервал допустимых значений [0 - FilesCount)");
            }
            else
            {
                if (temporaryFiles.ContainsKey(fileNumber))
                {
                    temporaryFiles[fileNumber] = value;
                }
                else
                {
                    temporaryFiles.Add(fileNumber, value);
                }
                numberOfActions++;
            }
        }

        public int Commit()
        {
            /*if (numberOfActions > MaxNumberOfActions)
            {
                return -1;
            }*/
            if(temporaryFiles.Count==0) //если временный словарь пуст
            {
                return -1;
            }
            if (Files.Count==0) //если хранилище пустое
            {
                foreach (var p in temporaryFiles)
                {
                    Files.Add(p.Key, new List<int>() { });
                    Files[p.Key].Insert(countOfCommit, p.Value);
                }
                numberOfActions++;
                return countOfCommit++;
            }
            else
            {
                bool isEqual = true;
                foreach (var p in temporaryFiles)
                {
                    if (temporaryFiles[p.Key] != Files[p.Key][countOfCommit - 1])
                    {
                        isEqual = false;
                        break;
                    }
                }
                if (isEqual)
                {
                    return -1;
                }
                else
                {
                    foreach (var p in temporaryFiles)
                    {
                        if (Files.ContainsKey(p.Key))
                        {
                            Files[p.Key].Insert(countOfCommit, p.Value);
                        }
                        else
                        {
                            Files.Add(p.Key, new List<int>() { });
                            Files[p.Key].Insert(countOfCommit, p.Value);
                        }
                    }
                    numberOfActions++;
                    return countOfCommit++;
                }
            }
        }

        public int Checkout(int commitNumber, int fileNumber)
        {
            /*if (numberOfActions > MaxNumberOfActions)
            {
                return -1;
            }*/
            if (fileNumber > FilesCount-1 || fileNumber < 0)
            {
                throw new ArgumentOutOfRangeException("fileNumber не попадает в интервал допустимых значений [0 - FilesCount)");
            }
            else if (commitNumber > countOfCommit || !Files.ContainsKey(fileNumber))
            {
                throw new ArgumentException("Данный коммит отсутствует");
            }
            else
            {
                numberOfActions++;
                temporaryFiles[fileNumber] = Files[fileNumber][commitNumber];
                return temporaryFiles[fileNumber];
            }
        }
    }
}