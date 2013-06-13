using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Spacebuilder.Common
{

    /// <summary>
    /// 生成随机值工具类
    /// </summary>
    public static class VerificationCodeRandomString
    {
        #region Private
        private static readonly Dictionary<CaptchaCharacterSet, Point> ranges = InitializeRanges();
        private static Dictionary<int, List<string>> ogdenBasicEnglishDictionary;
        private static int ogdenBasicEnglishDictionaryLongestWordLength;
        #endregion

        #region Methods
        /// <summary>
        /// Initializes the internal dictionary of character ranges that may be sampled at random.
        /// </summary>
        private static Dictionary<CaptchaCharacterSet, Point> InitializeRanges()
        {
            Dictionary<CaptchaCharacterSet, Point> ranges = new Dictionary<CaptchaCharacterSet, Point>();
            ranges[CaptchaCharacterSet.LowercaseLetters] = new Point((int)'a', (int)'z');
            ranges[CaptchaCharacterSet.UppercaseLetters] = new Point((int)'A', (int)'Z');
            ranges[CaptchaCharacterSet.Digits] = new Point((int)'0', (int)'9');
            return ranges;
        }

        /// <summary>
        /// Ensures that the words from the Ogden Basic English dictionary are indexed in memory by their lengths.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.Synchronized)]
        private static void EnsureOgdenBasicEnglishDictionary()
        {
            if (ogdenBasicEnglishDictionary != null)
                return;

            ogdenBasicEnglishDictionary = new Dictionary<int, List<string>>(16);

            string tempStr = "a,able,about,account,acid,across,act,addition,adjustment,advertisement,agreement,after,again,against,air,all,almost,among,amount,amusement,and,angle,angry,animal,answer,ant,any,apparatus,apple,approval,arch,argument,arm,army,art,as,at,attack,attempt,attention,attraction,authority,automatic,awake,baby,back,bad,bag,balance,ball,band,base,basin,basket,bath,be,beautiful,because,bed,bee,before,behavior,belief,bell,bent,berry,between,bird,birth,bit,bite,bitter,black,blade,blood,blow,blue,board,boat,body,boiling,bone,book,boot,bottle,box,boy,brain,brake,branch,brass,bread,breath,brick,bridge,bright,broken,brother,brown,brush,bucket,building,bulb,burn,burst,business,but,butter,button,by,cake,camera,canvas,card,care,carriage,cart,cat,cause,certain,chain,chalk,chance,change,cheap,cheese,chemical,chest,chief,chin,church,circle,clean,clear,clock,cloth,cloud,coal,coat,cold,collar,color,comb,come,comfort,committee,common,company,comparison,competition,complete,complex,condition,connection,conscious,control,cook,copper,copy,cord,cork,cotton,cough,country,cover,cow,crack,credit,crime,cruel,crush,cry,cup,current,curtain,curve,cushion,cut,damage,danger,dark,daughter,day,dead,dear,death,debt,decision,deep,degree,delicate,dependent,design,desire,destruction,detail,development,different,digestion,direction,dirty,discovery,discussion,disease,disgust,distance,distribution,division,do,dog,door,doubt,down,drain,drawer,dress,drink,driving,drop,dry,dust,ear,early,earth,east,edge,education,effect,egg,elastic,electric,end,engine,enough,equal,error,even,event,ever,every,example,exchange,existence,expansion,experience,expert,eye,face,fact,fall,false,family,far,farm,fat,father,fear,feather,feeble,feeling,female,fertile,fiction,field,fight,finger,fire,first,fish,fixed,flag,flame,flat,flight,floor,flower,fly,fold,food,foolish,foot,for,force,fork,form,forward,fowl,frame,free,frequent,friend,from,front,fruit,full,future,garden,general,get,girl,give,glass,glove,go,goat,gold,good,government,grain,grass,great,green,grey,gray,grip,group,growth,guide,gun,hair,hammer,hand,hanging,happy,harbor,hard,harmony,hat,hate,have,he,head,healthy,hearing,heart,heat,help,here,high,history,hole,hollow,hook,hope,horn,horse,hospital,hour,house,how,humor,I,ice,idea,if,ill,important,impulse,in,increase,industry,ink,insect,instrument,insurance,interest,invention,iron,island,jelly,jewel,join,journey,judge,jump,keep,kettle,key,kick,kind,kiss,knee,knife,knot,knowledge,land,language,last,late,laugh,law,lead,leaf,learning,leather,left,leg,let,letter,level,library,lift,light,like,limit,line,linen,lip,liquid,list,little,less,least,living,lock,long,loose,loss,loud,love,low,machine,make,male,man,manager,map,mark,market,married,match,material,mass,may,meal,measure,meat,medical,meeting,memory,metal,middle,military,milk,mind,mine,minute,mist,mixed,money,monkey,month,moon,morning,mother,motion,mountain,mouth,move,much,more,most,muscle,music,nail,name,narrow,nation,natural,near,necessary,neck,need,needle,nerve,net,new,news,night,no,noise,normal,north,nose,not,note,now,number,nut,observation,of,off,offer,office,oil,old,on,only,open,operation,opinion,opposite,or,orange,order,organization,ornament,other,out,oven,over,owner,page,pain,paint,paper,parallel,parcel,part,past,paste,payment,peace,pen,pencil,person,physical,picture,pig,pin,pipe,place,plane,plant,plate,play,please,pleasure,plough,plow,pocket,point,poison,polish,political,poor,porter,position,possible,pot,potato,powder,power,present,price,print,prison,private,probable,process,produce,profit,property,prose,protest,public,pull,pump,punishment,purpose,push,put,quality,question,quick,quiet,quite,rail,rain,range,rat,rate,ray,reaction,red,reading,ready,reason,receipt,record,regret,regular,relation,religion,representative,request,respect,responsible,rest,reward,rhythm,rice,right,ring,river,road,rod,roll,roof,room,root,rough,round,rub,rule,run,sad,safe,sail,salt,same,sand,say,scale,school,science,scissors,screw,sea,seat,second,secret,secretary,see,seed,selection,self,send,seem,sense,separate,serious,servant,sex,shade,shake,shame,sharp,sheep,shelf,ship,shirt,shock,shoe,short,shut,side,sign,silk,silver,simple,sister,size,skin,skirt,sky,sleep,slip,slope,slow,small,smash,smell,smile,smoke,smooth,snake,sneeze,snow,so,soap,society,sock,soft,solid,some,son,song,sort,sound,south,soup,space,spade,special,sponge,spoon,spring,square,stamp,stage,star,start,statement,station,steam,stem,steel,step,stick,sticky,still,stitch,stocking,stomach,stone,stop,store,story,strange,street,stretch,stiff,straight,strong,structure,substance,sugar,suggestion,summer,support,surprise,such,sudden,sun,sweet,swim,system,table,tail,take,talk,tall,taste,tax,teaching,tendency,test,than,that,the,then,theory,there,thick,thin,thing,this,though,thought,thread,throat,through,thumb,thunder,ticket,tight,till,time,tin,tired,to,toe,together,tomorrow,tongue,tooth,top,touch,town,trade,train,transport,tray,tree,trick,trouble,trousers,true,turn,twist,umbrella,under,unit,up,use,value,verse,very,vessel,view,violent,voice,waiting,walk,wall,war,warm,wash,waste,watch,water,wave,wax,way,weather,week,weight,well,west,wet,wheel,when,where,while,whip,whistle,white,who,why,wide,will,wind,window,wine,wing,winter,wire,wise,with,woman,wood,wool,word,work,worm,wound,writing,wrong,year,yellow,yes,yesterday,you,young";

            foreach (string word in tempStr.Split(',')) //Resources.Dictionaries.OgdenBasicEnglishAlphabetical.Split(','))
            {
                int length = word.Length;
                List<string> words;

                if (!ogdenBasicEnglishDictionary.ContainsKey(length))
                    ogdenBasicEnglishDictionary.Add(word.Length, words = new List<string>(64));
                else
                    words = ogdenBasicEnglishDictionary[length];

                words.Add(word);

                if (ogdenBasicEnglishDictionaryLongestWordLength < length)
                    ogdenBasicEnglishDictionaryLongestWordLength = length;
            }
        }

        /// <summary>
        /// Returns a word at random, from the Ogden Basic English dictionary, with a length that is between the specified "minLength and maxLengthvalues, inclusive.
        /// </summary>
        public static string CreateBasicEnglishWord(int minLength, int maxLength)
        {
            if (minLength < 0)
                throw new ArgumentOutOfRangeException("minLength", minLength, "Errors.PositiveIntOrZeroRequired");

            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException("maxLength", maxLength,
                    string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Errors.RandomString_MaxLessThanMin", minLength));

            Random rnd = new Random();

            int length = rnd.Next(minLength, maxLength + 1);

            if (length == 0)
                return string.Empty;

            EnsureOgdenBasicEnglishDictionary();

            List<string> words = ogdenBasicEnglishDictionary[Math.Min(length, ogdenBasicEnglishDictionaryLongestWordLength)];

            return words[rnd.Next(0, words.Count)];
        }

        /// <summary>
        /// Returns a string of random characters from the specified set of charactersAllowed, excluding 
        /// the characters in charactersNotAllowed, with a length that is between the 
        /// specified minLengthand maxLength values, inclusive.
        /// </summary>
        public static string Create(int minLength, int maxLength, CaptchaCharacterSet charactersAllowed, string charactersNotAllowed)
        {
            if (minLength < 0)
                throw new ArgumentOutOfRangeException("minLength", minLength, "Errors.PositiveIntOrZeroRequired");

            if (maxLength < minLength)
                throw new ArgumentOutOfRangeException("maxLength", maxLength,
                    string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Errors.RandomString_MaxLessThanMin", minLength));

            List<int> validCharSetValues = new List<int>((int[])Enum.GetValues(typeof(CaptchaCharacterSet)));

            for (int i = 0; i < validCharSetValues.Count; )
            // for each index in the array of CharacterSet constant values remove 
            // the ones that aren't bit-aligned or aren't selected in charSets
            {
                int csVal = validCharSetValues[i];

                if (!IsBitAligned(csVal) || (csVal & (int)charactersAllowed) == 0)
                {
                    validCharSetValues.RemoveAt(i);
                    continue;
                }

                i++;
            }

            Random rnd = new Random();

            int length = rnd.Next(minLength, maxLength + 1);

            if (length == 0)
                return string.Empty;

            StringBuilder value = new StringBuilder(length);

            if (!string.IsNullOrEmpty(charactersNotAllowed))
            {
                for (int i = 0; i < length; i++)
                // for each index in the string value generate a random character
                {
                    CaptchaCharacterSet validCharacters = (CaptchaCharacterSet)validCharSetValues[rnd.Next(0, validCharSetValues.Count)];

                    Point range = ranges[validCharacters];

                    char c = (char)rnd.Next(range.X, range.Y + 1);

                    if (charactersNotAllowed.IndexOf(c) == -1)		// case-sensitive comparison
                        value.Append(c);
                    else
                        // the current character must be replaced because it has been excluded
                        i--;
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                // for each index in the string value generate a random character
                {
                    // randomly choose a set of characters and get the character range for that set
                    Point range = ranges[(CaptchaCharacterSet)validCharSetValues[rnd.Next(0, validCharSetValues.Count)]];

                    value.Append((char)rnd.Next(range.X, range.Y + 1));
                }
            }

            return value.ToString();
        }

        /// <summary>
        /// Determines whether the specified value represents a number that exists on a binary boundary; 
        /// e.g., 1, 2, 4, 8, 16, 32, 64, etc.
        /// </summary>
        /// <param name="value">The number to check.</param>
        /// <returns><see langword="true">True</see> if the specified <paramref name="value"/> is bit-aligned; otherwise, 
        /// <see langword="false"/>.</returns>
        private static bool IsBitAligned(int value)
        {
            // WARN: not checking for value <= 0

            // must convert value to float so that division by 2 isn't rounded
            float fval = (float)value;

            do
            {
                if (fval == 1)
                    return true;
            }
            while ((fval /= 2) >= 1);

            return false;
        }
        #endregion
    }
}
