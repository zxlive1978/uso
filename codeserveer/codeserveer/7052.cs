using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;


namespace codeserveer
{
    public class _7052
    {

        //[7052]
        //DI_CHANNEL_COUNT=8
        //DO_CHANNEL_COUNT=0
        //AI_CHANNEL_COUNT=0
        //AO_CHANNEL_COUNT=0
        //COUNT_FREQ_CHANNEL_COUNT=0
        //DESCRIPTION=8*DI 
        //FORMDLL=UI_DIO
        //NAMESPACE=ICPDAS
        //CLASS=DIO
        //Номер порта
        public byte com=3;
        //Адрес
        public short address=7;
        //Скорость
        public uint baudrate=57600;
        //Слот
        public short slot=-1;
        //Контрольная сумма
        public short checksum=0;
        //Таймаут
        public short timeout=100;
        //Количество каналов
        public short totalchanels=8;
        //Тип канала
        public string chanels_type = "DI";
        //Имя устройства
        public string name = "";
        //Значения каналов
        public uint[] DIValue = new uint[8];
        //Состояние устройства
        public short iRet;


        //Время начало вычисления
        public long[] time_begin = new long[8];
        //Время начало вычисления
        public long[] time_cur = new long[8];
        //Время начало вычисления
        public int[] value_begin = new int[8];
        //Время начало вычисления
        public int[] value_cur = new int[8];
        //Начало времени
        public bool[] begin = new bool[8];
        //Счетчики
        public List <Stopwatch> stopWatch = new List<Stopwatch>();
        //public Stopwatch stopWatchs = new Stopwatch();
        //Частота
        public float[] freq2 = new float[8];
        public float[] freq1 = new float[8];
        //Минимальное время измерения
        public int time_min = 1000;
        //Максимальное время измерения
        public int time_max = 5000;

        //Список каналов
        public List<string> list_channels = new List<string>();

        //
        


        public _7052(){
            for (int i = 0; i < 8;i++ )
            {
                Stopwatch stopWatchs = new Stopwatch();
                stopWatch.Add(stopWatchs);
                begin[i] = true;
                DIValue[i] = 0;
                freq1[i] = 0;
                freq2[i] = 0;
            }
        
        }
    }
}
