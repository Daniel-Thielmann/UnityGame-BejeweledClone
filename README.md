# Bejeweled Clone

Este projeto é um protótipo do clássico jogo **Bejeweled**, desenvolvido como parte do **Trabalho E** da disciplina de **Desenvolvimento de Jogos** no curso de **Análise e Desenvolvimento de Sistemas** da **UFJF**. O objetivo é replicar as mecânicas principais do jogo original dentro da **Unity**, garantindo uma experiência funcional e fiel ao conceito original.

## 🎮 Funcionalidades Implementadas

✔️ **Detecção e eliminação de sequências de pedras**  
✔️ **Criação de novas pedras para preencher espaços vazios**  
✔️ **Controle de pontuação**  
✔️ **Sistema de power-ups**  
✔️ **Condição de derrota baseada no tempo esgotado**  

## 🛠️ Tecnologias Utilizadas

- **Unity Engine** (C#)
- **Sistema de Grid** para organização das peças
- **Lógica de detecção de combinações** via análise de vizinhança
- **Sistema de partículas e efeitos visuais** para melhorar a experiência do usuário

## 🎯 Objetivo do Jogo

O jogador deve trocar as gemas adjacentes para formar combinações de **três ou mais da mesma cor**. Quando uma combinação é feita, as pedras são eliminadas, e novas pedras caem do topo para preencher os espaços vazios. O jogo continua até o tempo acabar.

## 🌀 Power-Ups Implementados

- **Explosão** 💥: Remove todas as pedras ao redor da peça ativada.  
- **Linha/Coluna** ⚡: Elimina todas as pedras de uma linha ou coluna específica.  
- **Coringa** 🌈: Pode ser usado como qualquer cor para facilitar combinações.  

## ⏳ Condição de Derrota

O jogador deve alcançar a maior pontuação possível antes que o tempo se esgote. Caso o tempo acabe, a partida termina.

## 🚀 Como Jogar

1. Execute o jogo na Unity ou baixe o executável.
2. Clique em uma pedra e troque-a com uma adjacente para formar combinações.
3. Faça combinações consecutivas para acumular pontos e ativar power-ups.
4. Fique atento ao cronômetro! O jogo acaba quando o tempo zerar.

## 📁 Estrutura do Projeto

```
📂 Assets/Scripts/
├── ConfiguracoesGlobais.cs
├── ControladorDeTempo.cs
├── ControladorJogo.cs
├── ControladorPontuacaoFinal.cs
├── ControladorTelaFinal.cs
├── DadosElemento.cs
├── ElementoTabuleiro.cs
├── EnumeradoresJogo.cs
├── FerramentasAuxiliares.cs
├── FerramentasDebug.cs
├── GerenciadorTabuleiro.cs
├── RegistroCombinacoes.cs
├── RegistroMovimentacao.cs
```

## 🏆 Desenvolvimento

Este projeto foi desenvolvido como parte do Trabalho E da disciplina **Desenvolvimento de Jogos**, exigindo a replicação das mecânicas essenciais do **Bejeweled**, sem a necessidade de variações de dificuldade ou interfaces complexas.

## 📌 Créditos

**Desenvolvido por:** Daniel Alves Thielmann  
**Professor:** [Marcelo Caniato]  
**Curso:** Engenharia Computacional - UFJF  
