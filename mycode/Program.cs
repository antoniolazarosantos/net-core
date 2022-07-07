// See https://aka.ms/new-console-template for more information
using FluentColorConsole;

Console.WriteLine("Olá mundo!");

ShowMessage2();

void ShowMessage2(){
    Console.WriteLine("Outra mensagem.");
}


var textline = ColorConsole.WithRedText;
textline.WriteLine("FluentColorConsole");