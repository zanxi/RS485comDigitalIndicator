//#include "stdafx.h"
#include <conio.h>
#include <iostream>
#include <string>
#include <stdio>
#include <bitset>
#include <math.h>
using namespace std;





int main(int argc)
{

  char p[17];
itoa (1423, p, 16);
printf(p);

    float d=0x3FFFFF;
    d=0x7f6A3C3D;
    d=0x7f6A3C;
    //d=0x268d25;
    //d=12334;

    char temp[20];
    float znak;
    string c = itoa(d,temp,2);
        cout<<"\n";
    cout<<c;
            cout<<"\n";
            //c="001001101000110101010101";
            //c="100000000010100000000000";
    while (c.size()!=24)
    {
        c='0'+c;
    }
    if (c.c_str()[0]=='1')
    {
        znak=-1;
    }
    else
    {
        znak=1;
    }
    string mant;
    for (int i=8;i<c.size();i++)
        mant+=c[i];
            //cout<<"\n\n"<<mant<<"\n\n";
    std::bitset<16U> tr =bitset<16>(mant.c_str());
    string por;
    for (int i=1;i<8;i++)
        por+=c[i];
    std::bitset<7U> tr2 =bitset<7>(por.c_str());
    float r1=tr.to_ulong();
    float r2=tr2.to_ulong();
    float r3=pow(2,r2-64);
    float otv=znak*((r1/65536)*r3);
    cout << otv;
    getch();
    return 0;
}