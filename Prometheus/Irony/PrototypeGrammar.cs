using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;

namespace Prometheus.Irony
{
    class PrototypeGrammar :  Grammar
    {
        public PrototypeGrammar() : base(caseSensitive: false)
        {
            #region Regular Expressions

            RegexBasedTerminal realNum = new RegexBasedTerminal("REAL", @"[0-9]+\.[0-9]+");
            RegexBasedTerminal intNum = new RegexBasedTerminal("INTEGER", @"[0-9]+");
            RegexBasedTerminal aritmethic = new RegexBasedTerminal("ARITHMETIC", @"[\+\-/*%]");
            RegexBasedTerminal comparison = new RegexBasedTerminal("COMPARISON", @"==|<>|<=|>=|<|>");
            RegexBasedTerminal logical = new RegexBasedTerminal("LOGICAL", @"&&|\|\||!");
            RegexBasedTerminal str = new RegexBasedTerminal("STRING", "\".*\"|\'.*\'");
            IdentifierTerminal id = new IdentifierTerminal("ID");
            IdentifierTerminal libraryId = new IdentifierTerminal("LIBRARY-ID");
            IdentifierTerminal classId = new IdentifierTerminal("CLASS-ID");
            IdentifierTerminal functionId = new IdentifierTerminal("FUNCTION-ID");

            #endregion

            #region Terminals

            CommentTerminal lineComment = new CommentTerminal("LINE-COMMENT", "//", "\n", "\r\n");
            CommentTerminal blockComment = new CommentTerminal("BLOCK-COMMENT", "/*", "*/");
            
            // Operators, etc.
            
            KeyTerm plus = ToTerm("+");
            KeyTerm asterisc = ToTerm("*");
            KeyTerm increment = ToTerm("++");
            KeyTerm decrement = ToTerm("--");
            KeyTerm multincrement = ToTerm("+=");
            KeyTerm multidecrement = ToTerm("-=");
            KeyTerm assign = ToTerm("=");
            KeyTerm colon = ToTerm(":");
            KeyTerm semicolon = ToTerm(";");
            KeyTerm dot = ToTerm(".");
            KeyTerm comma = ToTerm(",");
            KeyTerm openBrace = ToTerm("{");
            KeyTerm closeBrace = ToTerm("}");
            KeyTerm openParenthesis = ToTerm("(");
            KeyTerm closeParenthesis = ToTerm(")");
            KeyTerm openBracket = ToTerm("[");
            KeyTerm closeBracket = ToTerm("]");

            // Reserved Words

            KeyTerm forTerminal = ToTerm("for");
            KeyTerm ifTerminal = ToTerm("if");
            KeyTerm caseTerminal = ToTerm("case");
            KeyTerm defaultTerminal = ToTerm("default");
            KeyTerm elseTerminal = ToTerm("else");
            KeyTerm continueTerminal = ToTerm("continue");
            KeyTerm breakTerminal = ToTerm("break");
            KeyTerm doTerminal = ToTerm("do");
            KeyTerm whileTerminal = ToTerm("while");
            KeyTerm switchTerminal = ToTerm("switch");

            KeyTerm trueTerminal = ToTerm("true");
            KeyTerm falseTerminal = ToTerm("false");
            KeyTerm nullTerminal = ToTerm("null");
            KeyTerm classTerminal = ToTerm("class");
            KeyTerm mainFunction = ToTerm("main");
            KeyTerm returnTerminal = ToTerm("return");

            KeyTerm publicTerminal = ToTerm("public");
            KeyTerm privateTerminal = ToTerm("private");
            KeyTerm protectedTerminal = ToTerm("protected");
            KeyTerm staticTerminal = ToTerm("static");
            KeyTerm thisTerminal = ToTerm("this");
            KeyTerm newTerminal = ToTerm("new");
            KeyTerm importTerminal = ToTerm("import");

            KeyTerm intData = ToTerm("int");
            KeyTerm longData = ToTerm("long");
            KeyTerm doubleData = ToTerm("double");
            KeyTerm floatData = ToTerm("float");
            KeyTerm stringData = ToTerm("String");
            KeyTerm boolData = ToTerm("boolean");
            KeyTerm byteData = ToTerm("byte");
            KeyTerm charData = ToTerm("char");
            KeyTerm voidTerminal = ToTerm("void");
            KeyTerm shortData = ToTerm("short");

            #endregion

            #region NonTerminals

            NonTerminal start = new NonTerminal("START");
            NonTerminal codeBlock = new NonTerminal("CODE-BLOCK");
            NonTerminal dataType = new NonTerminal("DATATYPE");
            NonTerminal methodDataType = new NonTerminal("METHOD-DATATYPE");
            NonTerminal accessType = new NonTerminal("ACCESS-TYPE");
            NonTerminal number = new NonTerminal("NUMBER");
            NonTerminal boolean = new NonTerminal("BOOLEAN");
            NonTerminal import = new NonTerminal("IMPORT");
            NonTerminal library = new NonTerminal("LIBRARY");
            NonTerminal variableCreation = new NonTerminal("VARIABLE-CREATION");
            NonTerminal variableNormal = new NonTerminal("VARIABLE-NORMAL");
            NonTerminal variableAsign = new NonTerminal("VARIABLE-ASIGN");
            NonTerminal value = new NonTerminal("VALUE");
            NonTerminal incDec = new NonTerminal("INCDEC");
            NonTerminal multiIncDec = new NonTerminal("MULTI-INCDEC");
            NonTerminal expression = new NonTerminal("EXPRESSION");
            NonTerminal numberExpression = new NonTerminal("NUMBER-EXPRESSION");
            NonTerminal strExpression = new NonTerminal("STR-EXPRESSION");
            NonTerminal objectExpression = new NonTerminal("OBJECT-EXPRESSION");
            NonTerminal classVariableExpression = new NonTerminal("CLASSVARIABLE-EXPRESSION");
            NonTerminal boolExpression = new NonTerminal("BOOL-EXPRESSION");
            NonTerminal idValue = new NonTerminal("IDVALUE");
            NonTerminal listIdValue = new NonTerminal("LIST-IDVALUE");
            NonTerminal arrayCreation = new NonTerminal("ARRAY-CREATION");
            NonTerminal arrayDeclaration = new NonTerminal("ARRAY-DECLARATION");
            NonTerminal controlStructure = new NonTerminal("CONTROL-STRUCTURE");
            NonTerminal ifStructure = new NonTerminal("IF-STRUCTURE");
            NonTerminal elseStructure = new NonTerminal("ELSE-STRUCTURE");
            NonTerminal switchStructure = new NonTerminal("SWITCH-STRUCTURE");
            NonTerminal boolNonTerminal = new NonTerminal("BOOL-NONTERMINAL");
            NonTerminal forStructure = new NonTerminal("FOR-STRUCTURE");
            NonTerminal caseStructure = new NonTerminal("CASE-STRUCTURE");
            NonTerminal classStructure = new NonTerminal("CLASS-STRUCTURE");
            NonTerminal funcStructure = new NonTerminal("FUNC-STRUCTURE");
            NonTerminal paramsList = new NonTerminal("PARAMS-LIST");
            NonTerminal mainFuncStructure = new NonTerminal("MAINFUNC-STRUCTURE");
            NonTerminal functionCodeBlock = new NonTerminal("FUNCTION-CODEBLOCK");
            NonTerminal mainCodeBlock = new NonTerminal("MAIN-CODEBLOCK");
            NonTerminal idValueExpression = new NonTerminal("IDVALUE-EXPRESSION");
            NonTerminal listIdValueExpression = new NonTerminal("LIST-IDVALUE-EXPRESSION");
            NonTerminal functionCall = new NonTerminal("FUNCTION-CALL");
            NonTerminal functionUse = new NonTerminal("FUNCTION-USE");
            NonTerminal innerCodeBlock = new NonTerminal("INNER-CODEBLOCK");

            #endregion

            #region Production Rules

            start.Rule = this.Empty
                | import + classStructure
                | classStructure;

            import.Rule = importTerminal + library + semicolon
                | importTerminal + library + import + semicolon;

            library.Rule = libraryId + dot + library
                | libraryId + dot + asterisc
                | libraryId;

            variableCreation.Rule = variableNormal + semicolon;

            variableNormal.Rule = dataType + id
                | dataType + id + assign + idValueExpression;

            variableAsign.Rule = id + assign + idValueExpression + semicolon
                | id + multiIncDec + idValueExpression + semicolon;

            value.Rule = number
                | str
                | boolData
                | nullTerminal;

            idValue.Rule = id
                | value;

            listIdValue.Rule = idValue
                | idValue + comma + listIdValue;

            idValueExpression.Rule = idValue
                | expression;

            listIdValueExpression.Rule = idValueExpression
                | idValueExpression + comma + listIdValueExpression;

            number.Rule = realNum 
                | intNum;

            dataType.Rule = intData
                | longData
                | doubleData
                | floatData
                | stringData
                | boolData
                | byteData
                | charData
                | shortData
                | id;

            methodDataType.Rule = intData
                | longData
                | doubleData
                | floatData
                | stringData
                | boolData
                | byteData
                | charData
                | shortData
                | voidTerminal
                | id;

            boolean.Rule = trueTerminal
                | falseTerminal;

            numberExpression.Rule = number
                | id
                | numberExpression + aritmethic + numberExpression
                | number + aritmethic + numberExpression
                | openParenthesis + numberExpression + closeParenthesis
                | number + incDec
                | incDec + number;

            incDec.Rule = increment
                | decrement;

            multiIncDec.Rule = multincrement
                | multidecrement;

            strExpression.Rule = value
                | id
                | strExpression + incDec + strExpression
                | openParenthesis + strExpression + closeParenthesis
                | strExpression + plus + strExpression;

            classVariableExpression.Rule = classId + dot + id
                | classId + dot + classVariableExpression;

            objectExpression.Rule = classId + openParenthesis + closeParenthesis
                | classId + dot + objectExpression
                | classId + openParenthesis + classVariableExpression + closeParenthesis
                | classId + openParenthesis + listIdValue + closeParenthesis
                | classId + openParenthesis + objectExpression + closeParenthesis;

            boolExpression.Rule = id
                | trueTerminal
                | falseTerminal
                | boolNonTerminal + comparison + boolNonTerminal
                | boolNonTerminal + comparison + boolNonTerminal + logical + boolExpression;

            boolNonTerminal.Rule = idValue
                | expression
                | trueTerminal
                | falseTerminal;

            expression.Rule = numberExpression
                | strExpression
                | newTerminal + objectExpression;

            arrayCreation.Rule = dataType + arrayDeclaration + semicolon;

            arrayDeclaration.Rule =  openBracket + closeBracket + id
                | id + openBracket + closeBracket
                | id + openBracket + closeBracket + assign + openBracket + closeBracket
                | id + openBracket + closeBracket + assign + openBracket + intNum + closeBracket
                | openBracket + closeBracket + id + assign + openBracket + id + closeBracket
                | openBracket + closeBracket + id + assign + openBracket + closeBracket
                | openBracket + closeBracket + id + assign + openBracket + intNum + closeBracket
                | openBracket + closeBracket + id + assign + openBracket + id + closeBracket;

            controlStructure.Rule = ifStructure
                | forStructure
                | switchTerminal;

            ifStructure.Rule = ifTerminal + idValue + openBrace + caseStructure + closeBrace
                | ifTerminal + boolExpression + openBrace + codeBlock + closeBrace
                | ifTerminal + boolExpression + openBrace + codeBlock + closeBrace + elseStructure
                | ifTerminal + idValue + openBrace + caseStructure + closeBrace + elseStructure;

            switchStructure.Rule = switchTerminal + idValue + openBrace + caseStructure + closeBrace;

            elseStructure.Rule = elseTerminal + openBrace + codeBlock + closeBrace
                | elseTerminal + ifStructure;

            caseStructure.Rule = caseTerminal + idValue + colon + codeBlock
                | caseTerminal + idValue + colon + codeBlock + caseStructure
                | defaultTerminal + colon + codeBlock;

            forStructure.Rule = forTerminal + boolExpression + openBrace + codeBlock + closeBrace
                | forTerminal + variableCreation + colon + idValue + comparison + idValue + colon + idValue + incDec + openBrace + codeBlock + closeBrace
                | forTerminal + variableCreation + colon + idValue + comparison + idValue + colon + idValue + incDec + idValue + openBrace + codeBlock + closeBrace;

            classStructure.Rule = accessType + classTerminal + classId + openBrace + mainCodeBlock + closeBrace
                | accessType + classTerminal + classId + openBrace + functionCodeBlock + closeBrace;

            mainFuncStructure.Rule = publicTerminal + staticTerminal + voidTerminal + mainFunction + openParenthesis + stringData + openBracket + closeBracket + "args" + closeParenthesis + openBrace + codeBlock + closeBrace
                | publicTerminal + staticTerminal + voidTerminal + mainFunction + openParenthesis + stringData + "args" + openBracket + closeBracket + closeParenthesis + openBrace + codeBlock + closeBrace;

            funcStructure.Rule = accessType + methodDataType + functionId + openParenthesis + closeParenthesis + openBrace + innerCodeBlock + closeBrace
                | accessType + methodDataType + functionId + openParenthesis + paramsList + closeParenthesis + openBrace + innerCodeBlock + closeBrace;

            accessType.Rule = publicTerminal
                | privateTerminal
                | protectedTerminal
                | publicTerminal + staticTerminal
                | privateTerminal + staticTerminal
                | protectedTerminal + staticTerminal;

            paramsList.Rule = dataType + id
                | dataType + id + comma + paramsList;

            functionUse.Rule = functionCall + semicolon;

            functionCall.Rule = functionId + dot + functionCall
                | functionId + openParenthesis + closeParenthesis
                | functionId + openParenthesis + listIdValueExpression + closeParenthesis
                | functionId + openParenthesis + functionCall + closeParenthesis;

            mainCodeBlock.Rule = mainFuncStructure
                | mainFuncStructure + functionCodeBlock
                | functionCodeBlock + mainFuncStructure;

            functionCodeBlock.Rule = funcStructure
                | funcStructure + functionCodeBlock;

            innerCodeBlock.Rule = returnTerminal
                | returnTerminal + idValueExpression
                | codeBlock
                | returnTerminal + innerCodeBlock
                | returnTerminal + idValueExpression + innerCodeBlock
                | codeBlock + innerCodeBlock;

            codeBlock.Rule = this.Empty
                | variableCreation
                | variableAsign
                | arrayDeclaration
                | controlStructure
                | functionUse
                | variableCreation + codeBlock
                | variableAsign + codeBlock
                | arrayDeclaration + codeBlock
                | controlStructure + codeBlock
                | functionUse + codeBlock;

            #endregion

            #region Preferences
            this.Root = start;
            this.NonGrammarTerminals.Add(lineComment);
            this.NonGrammarTerminals.Add(blockComment);
            #endregion
        }
    }
}
