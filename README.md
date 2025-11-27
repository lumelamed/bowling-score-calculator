# Bowling Score Calculator 🎳

Una aplicación de línea de comandos en .NET 8 para calcular puntuaciones de boliche de 10 pinos.

## 🚀 Características

- ✅ Calcula puntuaciones siguiendo las reglas oficiales de boliche
- ✅ Maneja strikes, spares y bonificaciones del frame 10
- ✅ Valida entrada y maneja errores gracefully
- ✅ Soporta múltiples jugadores
- ✅ Tests unitarios y de integración completos
- ✅ Arquitectura limpia y mantenible

## 📋 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## 🛠️ Instalación y Compilación

### Clonar/Descargar el proyecto

```bash
cd BowlingScoreCalculator
```

### Restaurar dependencias

```bash
dotnet restore
```

### Compilar el proyecto

```bash
dotnet build --configuration Release
```

### Ejecutar tests

```bash
# Todos los tests
dotnet test

# Con reporte de cobertura
dotnet test /p:CollectCoverage=true /p:CoverageReportFormat=opencover
```

## 🎮 Uso

### Ejecutar la aplicación

```bash
dotnet run --project src/BowlingScoreCalculator/BowlingScoreCalculator.csproj -- <archivo-entrada>
```

O después de compilar:

```bash
cd src/BowlingScoreCalculator/bin/Release/net8.0/
./BowlingScoreCalculator <archivo-entrada>
```

### Ejemplos

```bash
# Juego de ejemplo
dotnet run --project src/BowlingScoreCalculator/BowlingScoreCalculator.csproj -- test-files/sample-game.txt

# Juego perfecto
dotnet run --project src/BowlingScoreCalculator/BowlingScoreCalculator.csproj -- test-files/perfect-game.txt

# Juego con ceros
dotnet run --project src/BowlingScoreCalculator/BowlingScoreCalculator.csproj -- test-files/zero-game.txt
```

## 📝 Formato del Archivo de Entrada

El archivo debe contener líneas con el formato:

```
NombreJugador[TAB]Pinos
```

Donde:
- **NombreJugador**: Nombre del jugador (sin espacios en el TAB)
- **Pinos**: Número de pinos derribados (0-10) o 'F' para foul

### Ejemplo (sample-game.txt):

```
Jeff	10
John	3
John	7
Jeff	7
Jeff	3
...
```

## 📊 Formato de Salida

```
Frame		1		2		3		4		5		6		7		8		9		10
Jeff
Pinfalls	X	7	/	9	0	X	0	8	8	/	F	6	X	X	X	8	1
Score		20		39		48		66		74		84		90		120		148		167
John
Pinfalls	3	/	6	3	X	8	1	X	X	9	0	7	/	4	4	X	9	0
Score		16		25		44		53		82		101		110		124		132		151
```

## 🏗️ Arquitectura

### Estructura del Proyecto

```
src/
├── BowlingScoreCalculator/
│   ├── Models/              # Modelos de dominio
│   │   ├── Roll.cs
│   │   ├── Frame.cs
│   │   └── Player.cs
│   ├── Services/            # Lógica de negocio
│   │   ├── FileReader.cs
│   │   ├── GameParser.cs
│   │   ├── ScoreCalculator.cs
│   │   └── OutputFormatter.cs
│   ├── Exceptions/          # Excepciones personalizadas
│   │   └── InvalidInputException.cs
│   └── Program.cs           # Entry point
tests/
└── BowlingScoreCalculator.Tests/
    ├── Unit/                # Tests unitarios
    │   ├── ScoreCalculatorTests.cs
    │   ├── GameParserTests.cs
    │   └── FrameTests.cs
    └── Integration/         # Tests de integración
        └── EndToEndTests.cs
```

## 🧪 Testing

Se utiliza la librería de NUnit

### Cobertura de Tests

- ✅ Tests unitarios para lógica de cálculo
- ✅ Tests de validación de entrada
- ✅ Tests de casos edge (juego perfecto, ceros, fouls)
- ✅ Tests de integración end-to-end
